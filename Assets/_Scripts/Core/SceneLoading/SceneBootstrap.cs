using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
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
using MagmaHeart.Spawning;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private Player m_playerPrefab;
        [SerializeField] private CameraController m_cameraPrefab;
        [SerializeField] private DungeonGenerator m_locationGeneratorPrefab;
        [SerializeField] private GameUI m_uiPrefab;
        [SerializeField] private DungeonGrid m_gridPrefab;
        [SerializeField] private Teleporter m_teleporterPrefab;
        [SerializeField] private CombatAltar m_combatAltarPrefab;
        [SerializeField] private CombatTilemapRenderer m_combatTilemapRendererPrefab;
        [SerializeField] private MouseListener m_mouseListenerPrefab;

        [Header("SpawnService Settings")]
        [SerializeField] private GameObject m_projectilePrefab;
        [SerializeField] private List<GameObject> m_enemyPrefabs;
        [SerializeField] private float m_minDistanceFromPlayer;

        private SceneLoader m_sceneLoader;
        private Location m_location;
        private LocationRenderer m_renderer;
        private UserInput m_userInput;
        private DungeonGrid m_grid;
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

        public async void BootScene()
        {
            m_userInput = new UserInput();

            m_mouseListener = Instantiate(m_mouseListenerPrefab);
            m_mouseListener.Initialize(m_userInput);

            m_grid = Instantiate(m_gridPrefab);
            m_grid.Initialize();

            DungeonGenerator locationGeneratorInstance = Instantiate(m_locationGeneratorPrefab);
            m_renderer = locationGeneratorInstance.GetComponent<LocationRenderer>();

            m_renderer.RenderedAllTiles += BootSceneAfterRender;

            m_location = await locationGeneratorInstance.GenerateRoom(Vector2Int.zero);
            m_renderer.AddTilesToDraw(m_location.FloorTiles, m_grid.Floor, m_grid.FloorTile);
            m_renderer.AddTilesToDraw(m_location.WallTiles, m_grid.Walls, m_grid.WallTile);
            m_renderer.DrawTiles();
        }

        private void BootSceneAfterRender()
        {
            m_renderer.RenderedAllTiles -= BootSceneAfterRender;

            ActionDatabase database = new ActionDatabase(Assembly.GetExecutingAssembly());
            ActionSelector actionSelectorChain = new AttackActionSelector(database.Get<AttackAction>());
            actionSelectorChain.Next = new MovementActionSelector(database.Get<MovementAction>());
            ActionPreviewService previewService = new ActionPreviewService(actionSelectorChain);

            RoomModel startRoom = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];
            Player spawnedPlayer = Instantiate(m_playerPrefab, (Vector2)startRoom.WorldPosition, Quaternion.identity);
            spawnedPlayer.Initialize(m_userInput, m_mouseListener, m_grid, previewService);

            if (m_sceneLoader.SavedData != null)
            {
                SaveData savedData = m_sceneLoader.SavedData;

                spawnedPlayer.Health.CurrentHealth = savedData.health;
            }

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
            EnemySpawnContextFactory factory = new EnemySpawnContextFactory(m_grid, m_combatAI);
            
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

            m_camera = Instantiate(m_cameraPrefab, new Vector3(startRoom.WorldPosition.x, startRoom.WorldPosition.y, -10), Quaternion.identity);
            m_camera.Initialize(spawnedPlayer.transform, m_userInput, m_battle);

            InitializeStateMachine(spawnedPlayer);
            m_gameUI.RewardUI.Initialize(m_battleReward);

            InitializeCombatSystem(startRoom);
        }

        private void InitializeStateMachine(Player player)
        {
            ActionState actionState = new ActionState(player.Controller, m_hoverModeController);
            CombatState combatState = new CombatState(m_camera, m_grid, m_hoverModeController, m_battle, player);

            ArtifactDatabase database = new ArtifactDatabase();
            m_battleReward = new BattleReward(database);

            RewardState rewardState = new RewardState(m_battleReward, player);

            m_stateMachine = new GameStateMachine(actionState, combatState, rewardState);
            m_battle.OnBattleStarted += m_stateMachine.HandleOnBattleStarted;
            m_battle.OnBattleEnded += m_stateMachine.HandleOnBattleEnded;
            m_gameUI.RewardUI.OnRewardPicked += m_stateMachine.HandleOnRewardPicked;
        }

        private void InitializeCombatSystem(RoomModel startRoom)
        {
            RoomModel bossRoom = m_location.GetFarthestRoomFrom(startRoom);

            foreach (RoomModel roomTileData in m_location.Rooms)
            {
                if (roomTileData != startRoom)
                {
                    CombatTilemapRenderer renderer = Instantiate(m_combatTilemapRendererPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                    BoardGraph graph = BoardGraphBuilder.GenerateBoardGraph(roomTileData);
                    Room room = new Room(roomTileData, m_grid, renderer, graph);

                    if (roomTileData != bossRoom)
                    {
                        CombatAltar altarInstance = Instantiate(m_combatAltarPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                        altarInstance.Initialize(room, m_battle);
                    }
                }
            }
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