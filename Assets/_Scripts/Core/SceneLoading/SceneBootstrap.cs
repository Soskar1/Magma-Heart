using MagmaHeart.AI.Actions;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.Spawning;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.Spawning;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private CameraController m_cameraPrefab;
        [SerializeField] private GameUI m_uiPrefab;
        [SerializeField] private CombatTilemapRenderer m_combatTilemapRendererPrefab;
        [SerializeField] private MouseListener m_mouseListenerPrefab;

        [Header("Dungeon")]
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

        private SceneLoader m_sceneLoader;
        private UserInput m_userInput;
        private CameraController m_camera;

        private GameUI m_gameUI;
        private Battle m_battle;
        private BattleReward m_battleReward;
        private GameStateMachine m_stateMachine;
        private CombatAI m_combatAI;

        private Inventory m_inventory;
        private HoverModeController m_hoverModeController;
        private MouseListener m_mouseListener;

        public void Initialize(SceneLoader sceneLoader) => m_sceneLoader = sceneLoader;

        public async void Awake()
        {
            await BootScene();
        }

        public async Task BootScene()
        {
            // Input
            m_userInput = new UserInput();

            m_mouseListener = Instantiate(m_mouseListenerPrefab);
            m_mouseListener.Initialize(m_userInput);

            //// Dungeon
            System.Random random = new System.Random(m_seed);
            TextAsset configFile = ExternalResources.LoadTextAsset(m_configFileName);
            DungeonGeneratorData data = DungeonGeneratorDataDeserializer.Deserialize(configFile, random);
            DungeonGenerator dungeonGenerator = new DungeonGenerator(data);
            m_roomRenderer.Initialize(m_dungeonTilemap);

            RoomModel roomModel = dungeonGenerator.GenerateRoom();
            await m_roomRenderer.DrawRoom(roomModel, m_floorTile, m_wallTile);

            m_roomGrid = new RoomGrid(m_dungeonGrid, m_dungeonTilemap);

            CombatTilemapRenderer renderer = Instantiate(m_combatTilemapRendererPrefab, roomModel.WorldPosition.ToVector3(), Quaternion.identity);
            Room room = new Room(roomModel, m_roomGrid, renderer);
            
            /////

            ActionDatabase database = new ActionDatabase(Assembly.GetExecutingAssembly());
            ActionSelector actionSelectorChain = new AttackActionSelector(database.Get<AttackAction>());
            actionSelectorChain.Next = new MovementActionSelector(database.Get<MovementAction>());
            ActionPreviewService previewService = new ActionPreviewService(actionSelectorChain);

            Player spawnedPlayer = Instantiate(m_playerPrefab, (Vector2)roomModel.WorldPosition, Quaternion.identity);
            spawnedPlayer.Initialize(m_userInput, m_mouseListener, m_roomGrid, previewService);

            AggressiveStrategy strategy = new AggressiveStrategy();
            
            m_combatAI = new CombatAI(strategy, database, 2);

            Dictionary<GameObject, SpawnConfig> configs = new Dictionary<GameObject, SpawnConfig>();
            foreach (GameObject prefab in m_enemyPrefabs)
            {
                EnemySpawnConfig config = new EnemySpawnConfig(prefab);
                configs.Add(prefab, config);
            }
            ProjectileSpawnConfig projectileConfig = new ProjectileSpawnConfig(m_projectilePrefab);
            configs.Add(m_projectilePrefab, projectileConfig);

            SpawnService spawnService = new SpawnService(configs, new UnityInstantiator());
            EnemySpawnContextFactory factory = new EnemySpawnContextFactory(m_roomGrid, m_combatAI);
            
            EnemySpawner enemySpawner = new EnemySpawner(spawnService, spawnedPlayer, m_minDistanceFromPlayer, m_enemyPrefabs, factory);
            ProjectileSpawner projectileSpawner = new ProjectileSpawner(spawnService, m_projectilePrefab);

            MagmaHeartSpawner spawner = new MagmaHeartSpawner(enemySpawner, projectileSpawner);

            m_battle = new Battle(spawnedPlayer, spawner);
            m_battle.OnBattleStarted += m_combatAI.HandleOnBattleStarted;
            
            MouseHoverEngine hoverEngine = new MouseHoverEngine(m_mouseListener);
            m_gameUI = Instantiate(m_uiPrefab);

            m_hoverModeController = new HoverModeController(hoverEngine, m_battle, m_gameUI.Raycaster, spawnedPlayer.CombatController);
            m_gameUI.Initialize(spawnedPlayer, m_battle, hoverEngine);

            m_inventory = new Inventory(spawnedPlayer.Model, m_gameUI.RewardUI);

            m_camera = Instantiate(m_cameraPrefab, new Vector3(roomModel.WorldPosition.x, roomModel.WorldPosition.y, -10), Quaternion.identity);
            m_camera.Initialize(spawnedPlayer.transform, m_userInput, m_battle);

            InitializeStateMachine(spawnedPlayer);
            m_gameUI.RewardUI.Initialize(m_battleReward);
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
            m_battle.OnBattleStarted -= m_combatAI.HandleOnBattleStarted;
            m_battle.OnBattleStarted -= m_stateMachine.HandleOnBattleStarted;
            m_battle.OnBattleEnded -= m_stateMachine.HandleOnBattleEnded;
            m_gameUI.RewardUI.OnRewardPicked -= m_stateMachine.HandleOnRewardPicked;

            m_hoverModeController.Disable();
            m_inventory.Disable();
        }
    }
}