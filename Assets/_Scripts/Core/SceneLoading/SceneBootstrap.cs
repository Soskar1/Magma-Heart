using MagmaHeart.Core.AI;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.Spawning;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.DungeonGeneration;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private CameraController m_cameraPrefab;
        [SerializeField] private GameUI m_uiPrefab;
        [SerializeField] private MouseListener m_mouseListenerPrefab;

        [Header("Dungeon")]
        [SerializeField] private CombatTilemapRenderer m_combatTilemapRenderer;
        [SerializeField] private Tilemap m_dungeonTilemap;
        [SerializeField] private Grid m_dungeonGrid;
        [SerializeField] private RoomRenderer m_roomRenderer;
        [SerializeField] private int m_seed;
        [SerializeField] private string m_configFileName;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;
        private RoomGrid m_roomGrid;

        [Header("SpawnService Settings")]
        [SerializeField] private GameObject m_projectilePrefab;
        [SerializeField] private List<GameObject> m_enemyPrefabs;
        [SerializeField] private float m_minDistanceFromPlayer;

        [Header("UI")]
        [SerializeField] private GraphicRaycaster m_graphicRaycaster;

        private SceneLoader m_sceneLoader;
        private CameraController m_camera;

        private GameUI m_gameUI;
        private Battle m_battle;
        private BattleReward m_battleReward;
        private GameStateMachine m_stateMachine;

        private Inventory m_inventory;
        private HoverModeController m_hoverModeController;

        private InputInstaller m_inputInstaller;
        private DungeonInstaller m_dungeonInstaller;
        private AIInstaller m_aiInstaller;
        private PlayerInstaller m_playerInstaller;
        private SpawnerInstaller m_spawnerInstaller;

        private AIContext m_aiContext;

        public void Initialize(SceneLoader sceneLoader) => m_sceneLoader = sceneLoader;

        public async void Awake()
        {
            await BootScene();
        }

        public async Task BootScene()
        {
            m_inputInstaller = new InputInstaller();
            InputContext inputContext = m_inputInstaller.Install(m_mouseListenerPrefab);

            System.Random random = new System.Random(m_seed);
            m_dungeonInstaller = new DungeonInstaller();
            DungeonGenerator dungeonGenerator = m_dungeonInstaller.Install(random, m_configFileName);

            // I think, we will need to move this to another place
            RoomModel roomModel = dungeonGenerator.GenerateRoom();
            await m_roomRenderer.DrawRoom(roomModel, m_floorTile, m_wallTile);

            m_roomGrid = new RoomGrid(m_dungeonGrid, m_dungeonTilemap);
            Room room = new Room(roomModel, m_roomGrid, m_combatTilemapRenderer);
            //

            m_aiInstaller = new AIInstaller();
            m_aiContext = m_aiInstaller.Install();

            m_playerInstaller = new PlayerInstaller();
            Player player = m_playerInstaller.Install(m_playerPrefab, inputContext, m_aiContext.ActionDatabase, m_roomGrid, roomModel.WorldPosition);

            m_spawnerInstaller = new SpawnerInstaller();
            MagmaHeartSpawner spawner = m_spawnerInstaller.Install(m_enemyPrefabs, player, m_projectilePrefab, m_aiContext, m_roomGrid, m_minDistanceFromPlayer);

            m_battle = new Battle(player, spawner);
            m_battle.OnBattleStarted += m_aiContext.CombatAI.HandleOnBattleStarted;
            
            m_hoverModeController = new HoverModeController(inputContext.MouseHoverEngine, m_battle, m_graphicRaycaster, player.CombatController);

            m_gameUI = Instantiate(m_uiPrefab);
            m_gameUI.Initialize(player, m_battle, inputContext.MouseHoverEngine);

            m_inventory = new Inventory(player.Model, m_gameUI.RewardUI);

            m_camera = Instantiate(m_cameraPrefab, new Vector3(roomModel.WorldPosition.x, roomModel.WorldPosition.y, -10), Quaternion.identity);
            m_camera.Initialize(player.transform, inputContext.UserInput, m_battle);

            InitializeStateMachine(player);
            m_gameUI.RewardUI.Initialize(m_battleReward);

            await m_battle.Start(room);
        }

        private void InitializeStateMachine(Player player)
        {
            ActionState actionState = new ActionState(player.Controller, m_hoverModeController);
            CombatState combatState = new CombatState(m_camera, m_roomGrid, m_hoverModeController, m_battle, player);

            ArtifactDatabase database = new ArtifactDatabase();
            m_battleReward = new BattleReward(database);

            RewardState rewardState = new RewardState(m_battleReward, player);

            m_stateMachine = new GameStateMachine(actionState, combatState, rewardState);
            m_battle.OnBattleStarted += m_stateMachine.HandleOnBattleStarted;
            m_battle.OnBattleEnded += m_stateMachine.HandleOnBattleEnded;
            m_gameUI.RewardUI.OnRewardPicked += m_stateMachine.HandleOnRewardPicked;
        }

        public void OnDisable()
        {
            m_inputInstaller.Dispose();
            m_dungeonInstaller.Dispose();
            m_aiInstaller.Dispose();
            m_playerInstaller.Dispose();

            m_battle.OnBattleStarted -= m_aiContext.CombatAI.HandleOnBattleStarted;
            m_battle.OnBattleStarted -= m_stateMachine.HandleOnBattleStarted;
            m_battle.OnBattleEnded -= m_stateMachine.HandleOnBattleEnded;
            m_gameUI.RewardUI.OnRewardPicked -= m_stateMachine.HandleOnRewardPicked;

            m_hoverModeController.Disable();
            m_inventory.Disable();
        }
    }
}