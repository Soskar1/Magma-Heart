using MagmaHeart.Core.AI;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Preview;
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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private CameraController m_cameraPrefab;

        [Header("Input")]
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

        [Header("SpawnService Settings")]
        [SerializeField] private GameObject m_projectilePrefab;
        [SerializeField] private List<GameObject> m_enemyPrefabs;
        [SerializeField] private float m_minDistanceFromPlayer;

        [Header("UI")]
        [SerializeField] private GameUI m_gameUI;
        [SerializeField] private GraphicRaycaster m_graphicRaycaster;

        [Header("Travel")]
        [SerializeField] private int m_travelSpeed;

        private Battle m_battle;

        private Inventory m_inventory;
        private HoverModeController m_hoverModeController;

        private InputInstaller m_inputInstaller;
        private DungeonInstaller m_dungeonInstaller;
        private AIInstaller m_aiInstaller;
        private PlayerInstaller m_playerInstaller;
        private SpawnerInstaller m_spawnerInstaller;
        private ActionPreviewInstaller m_actionPreviewInstaller;

        private AIContext m_aiContext;

        public async void Awake()
        {
            await BootScene();
        }

        public async Task BootScene()
        {
            m_inputInstaller = new InputInstaller();
            InputContext inputContext = m_inputInstaller.Install(m_mouseListenerPrefab);

            if (m_seed == -1)
                m_seed = Environment.TickCount;

            System.Random random = new System.Random(m_seed);

            m_dungeonInstaller = new DungeonInstaller();
            DungeonGenerator dungeonGenerator = m_dungeonInstaller.Install(random, m_configFileName);

            RoomGrid grid = new RoomGrid(m_dungeonGrid, m_dungeonTilemap);
            DungeonController dungeonController = new DungeonController(dungeonGenerator, grid);
            m_roomRenderer.Initialize(dungeonController);

            m_aiInstaller = new AIInstaller();
            m_aiContext = m_aiInstaller.Install();

            m_spawnerInstaller = new SpawnerInstaller();
            MagmaHeartSpawner spawner = m_spawnerInstaller.Install(m_enemyPrefabs, m_projectilePrefab, m_aiContext, grid, m_minDistanceFromPlayer);

            EntityMovementService entityMovementService = new EntityMovementService();
            m_battle = new Battle(spawner, entityMovementService);
            m_battle.OnBattleStarted += m_aiContext.CombatAI.HandleOnBattleStarted;

            m_actionPreviewInstaller = new ActionPreviewInstaller(m_combatTilemapRenderer);
            IActionPreviewProvider previewProvider = m_actionPreviewInstaller.Install(m_aiContext.ActionDatabase, m_battle, dungeonController);

            m_playerInstaller = new PlayerInstaller();
            Player player = m_playerInstaller.Install(m_playerPrefab, inputContext, grid, previewProvider);
            player.gameObject.SetActive(false);

            CameraController camera = Instantiate(m_cameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            camera.Initialize(player.transform, inputContext.UserInput, m_battle);

            m_hoverModeController = new HoverModeController(inputContext.MouseHoverEngine, dungeonController, m_graphicRaycaster, previewProvider, m_combatTilemapRenderer);
            m_hoverModeController.UseRaycastHover();

            ArtifactDatabase database = new ArtifactDatabase();
            BattleReward battleReward = new BattleReward(database);

            m_gameUI.Initialize(player, m_battle, inputContext.MouseHoverEngine, battleReward, previewProvider);
            m_inventory = new Inventory(player.Model, m_gameUI.RewardUI);

            MagmaHeartContext magmaHeartContext = new MagmaHeartContext(dungeonController, player, m_hoverModeController, entityMovementService, camera, m_battle, battleReward, m_gameUI);
            MagmaHeartStateMachine stateMachine = new MagmaHeartStateMachine(magmaHeartContext, m_travelSpeed);
            await stateMachine.Start();
        }

        public void OnDisable()
        {
            m_inputInstaller.Dispose();
            m_dungeonInstaller.Dispose();
            m_aiInstaller.Dispose();
            m_playerInstaller.Dispose();
            m_spawnerInstaller.Dispose();
            m_actionPreviewInstaller.Dispose();

            m_battle.OnBattleStarted -= m_aiContext.CombatAI.HandleOnBattleStarted;

            m_hoverModeController.Disable();
            m_inventory.Disable();
        }
    }
}