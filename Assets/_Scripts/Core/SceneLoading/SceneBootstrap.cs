using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.Core.Presentation.UI;
using UnityEngine;
using MagmaHeart.AI.Boards;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private CameraController m_cameraPrefab;
        [SerializeField] private LocationGenerator m_locationGeneratorPrefab;
        [SerializeField] private GameUI m_uiPrefab;
        [SerializeField] private DungeonGrid m_gridPrefab;
        [SerializeField] private Teleporter m_teleporterPrefab;
        [SerializeField] private CombatAltar m_combatAltarPrefab;
        [SerializeField] private CombatTilemapRenderer m_combatTilemapRendererPrefab;
        [SerializeField] private MousePositionListener m_mousePositionListenerPrefab;

        [Header("Spawner Settings")]
        [SerializeField] private Enemy m_enemyPrefab;
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
        private MouseHover m_mouseHover;

        public void Initialize(SceneLoader sceneLoader) => m_sceneLoader = sceneLoader;

        public async void BootScene()
        {
            m_userInput = new UserInput();

            m_grid = Instantiate(m_gridPrefab);
            m_grid.Initialize();

            LocationGenerator locationGeneratorInstance = Instantiate(m_locationGeneratorPrefab);
            m_renderer = locationGeneratorInstance.GetComponent<LocationRenderer>();

            m_renderer.RenderedAllTiles += BootSceneAfterRender;

            m_location = await locationGeneratorInstance.GenerateLocation(Vector2Int.zero);
            m_renderer.AddTilesToDraw(m_location.FloorTiles, m_grid.Floor, m_grid.FloorTile);
            m_renderer.AddTilesToDraw(m_location.WallTiles, m_grid.Walls, m_grid.WallTile);
            m_renderer.AddTilesToDraw(m_location.CorridorEntranceTiles, m_grid.Corridors, m_grid.WallTile);
            m_renderer.DrawTiles();
        }

        private void BootSceneAfterRender()
        {
            m_renderer.RenderedAllTiles -= BootSceneAfterRender;

            RoomTileData startRoom = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];
            Player spawnedPlayer = Instantiate(m_player, (Vector2)startRoom.WorldPosition, Quaternion.identity);
            spawnedPlayer.Initialize(m_userInput, m_grid);

            if (m_sceneLoader.SavedData != null)
            {
                SaveData savedData = m_sceneLoader.SavedData;

                spawnedPlayer.Health.CurrentHealth = savedData.health;
            }

            AggressiveStrategy strategy = new AggressiveStrategy(2, spawnedPlayer.Model);
            m_combatAI = new CombatAI(strategy);

            Spawner spawner = new Spawner(spawnedPlayer, m_enemyPrefab, m_minDistanceFromPlayer, m_grid, m_combatAI);
            m_battle = new Battle(spawnedPlayer, spawner);
            m_battle.OnBattleStarted += m_combatAI.HandleOnBattleStarted;

            m_gameUI = Instantiate(m_uiPrefab);
            m_gameUI.Initialize(spawnedPlayer, m_battle);

            m_inventory = new Inventory(spawnedPlayer.Model, m_gameUI.RewardUI);

            m_camera = Instantiate(m_cameraPrefab, new Vector3(startRoom.WorldPosition.x, startRoom.WorldPosition.y, -10), Quaternion.identity);
            m_camera.Initialize(spawnedPlayer.transform, m_userInput, m_battle);

            InitializeStateMachine(spawnedPlayer);
            m_gameUI.RewardUI.Initialize(m_battleReward);

            InitializeCombatSystem(startRoom, m_stateMachine);
        }

        private void InitializeStateMachine(Player player)
        {
            MousePositionListener mousePositionListener = Instantiate(m_mousePositionListenerPrefab);
            mousePositionListener.Initialize(m_userInput);
            m_mouseHover = new MouseHover(mousePositionListener, (PlayerTurnContext)player.TurnContext, m_battle);

            ActionState actionState = new ActionState(player.Controller, m_mouseHover);
            CombatState combatState = new CombatState(m_camera, m_grid, m_mouseHover);

            ArtifactDatabase database = new ArtifactDatabase();
            m_battleReward = new BattleReward(database);

            RewardState rewardState = new RewardState(m_battleReward, player);

            m_stateMachine = new GameStateMachine(actionState, combatState, rewardState);
            m_battle.OnBattleStarted += m_stateMachine.HandleOnBattleStarted;
            m_battle.OnBattleEnded += m_stateMachine.HandleOnBattleEnded;
            m_gameUI.RewardUI.OnRewardPicked += m_stateMachine.HandleOnRewardPicked;
        }

        private void InitializeCombatSystem(RoomTileData startRoom, GameStateMachine stateMachine)
        {
            RoomTileData bossRoom = m_location.GetFarthestRoomFrom(startRoom);

            foreach (RoomTileData roomTileData in m_location.Rooms)
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

            m_mouseHover.Disable();
            m_inventory.Disable();
        }
    }
}