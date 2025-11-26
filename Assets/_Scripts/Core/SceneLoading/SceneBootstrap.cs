using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.Core.UI;
using UnityEngine;
using MagmaHeart.AI.Boards;
using MagmaHeart.Core.AI;
using System.Linq;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities.Presenters;

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

        public void Initialize(SceneLoader sceneLoader) => m_sceneLoader = sceneLoader;

        public async void BootScene()
        {
            m_userInput = new UserInput();

            m_grid = Instantiate(m_gridPrefab);
            m_grid.Initialize();

            LocationGenerator locationGeneratorInstance = Instantiate(m_locationGeneratorPrefab);
            m_renderer = locationGeneratorInstance.GetComponent<LocationRenderer>();

            m_renderer.RenderedAllTiles += InitializePlayer;

            m_location = await locationGeneratorInstance.GenerateLocation(Vector2Int.zero);
            m_renderer.AddTilesToDraw(m_location.FloorTiles, m_grid.Floor, m_grid.FloorTile);
            m_renderer.AddTilesToDraw(m_location.WallTiles, m_grid.Walls, m_grid.WallTile);
            m_renderer.AddTilesToDraw(m_location.CorridorEntranceTiles, m_grid.Corridors, m_grid.WallTile);
            m_renderer.DrawTiles();
        }

        private void InitializePlayer()
        {
            m_renderer.RenderedAllTiles -= InitializePlayer;

            RoomTileData startRoom = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];

            m_gameUI = Instantiate(m_uiPrefab);
            Player spawnedPlayer = SpawnPlayer(startRoom);

            m_gameUI.Initialize(spawnedPlayer);
            m_gameUI.HealthBar.gameObject.SetActive(true);

            if (m_sceneLoader.SavedData != null)
            {
                SaveData savedData = m_sceneLoader.SavedData;

                spawnedPlayer.Health.CurrentHealth = savedData.health;
            }

            AggressiveStrategy strategy = new AggressiveStrategy(2, spawnedPlayer.Model);
            m_combatAI = new CombatAI(strategy);

            List<ITurnSwitchListener> turnListeners = new List<ITurnSwitchListener>()
            {
                m_camera.TurnSwitchListener, m_gameUI
            };

            Spawner spawner = new Spawner(spawnedPlayer, m_enemyPrefab, m_minDistanceFromPlayer, m_grid, m_combatAI);
            m_battle = new Battle(spawnedPlayer, spawner, turnListeners);
            m_battle.OnBattleStarted += m_combatAI.HandleOnBattleStarted;

            InitializeStateMachine(spawnedPlayer);
            m_gameUI.RewardUI.Initialize(m_stateMachine);

            InitializeCombatSystem(startRoom, m_stateMachine);
        }

        private Player SpawnPlayer(RoomTileData startRoom)
        {
            ActionUserInput actionUserInput = new ActionUserInput(m_userInput);

            List<MouseOverUIElement> mouseOverUIEvents = m_gameUI.GetComponentsInChildren<MouseOverUIElement>(true).ToList();
            CombatUserInput turnBasedUserInput = new CombatUserInput(m_userInput, m_grid, mouseOverUIEvents);

            Player playerInstance = Instantiate(m_player, (Vector2)startRoom.WorldPosition, Quaternion.identity);
            playerInstance.Initialize(actionUserInput, turnBasedUserInput, m_gameUI, m_grid);

            m_camera = Instantiate(m_cameraPrefab, new Vector3(startRoom.WorldPosition.x, startRoom.WorldPosition.y, -10), Quaternion.identity);
            m_camera.Initialize(playerInstance.transform, actionUserInput, turnBasedUserInput);

            playerInstance.Enable();

            return playerInstance;
        }

        private void InitializeStateMachine(Player player)
        {
            List<IActionStateListener> actionListeners = new List<IActionStateListener>()
            { player };

            ActionState actionState = new ActionState(actionListeners);

            List<ICombatStateListener> combatStateListeners = new List<ICombatStateListener>()
            {
                player, m_camera, m_grid
            };
            
            CombatState combatState = new CombatState(m_battle, combatStateListeners);

            ArtifactDatabase database = new ArtifactDatabase();
            m_battleReward = new BattleReward(database);
            m_battleReward.OnBattleRewardCalculated += m_gameUI.RewardUI.HandleOnBattleRewardCalculated;

            List<IRewardStateListener> rewardStateListeners = new List<IRewardStateListener>()
            {
                player, m_gameUI.RewardUI, m_battleReward
            };
            RewardState rewardState = new RewardState(rewardStateListeners);

            m_stateMachine = new GameStateMachine(actionState, combatState, rewardState);
            m_battle.OnBattleEnded += m_stateMachine.HandleOnBattleEnded;
            m_battle.OnBattleEnded += m_gameUI.HandleOnBattleEnded;
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
                        altarInstance.Initialize(room, stateMachine);
                    }
                }
            }
        }

        public void OnDisable()
        {
            m_battle.Disable();
            m_battle.OnBattleStarted -= m_combatAI.HandleOnBattleStarted;
            m_battle.OnBattleEnded -= m_stateMachine.HandleOnBattleEnded;
            m_battle.OnBattleEnded -= m_gameUI.HandleOnBattleEnded;
            m_battleReward.OnBattleRewardCalculated -= m_gameUI.RewardUI.HandleOnBattleRewardCalculated;
        }
    }
}