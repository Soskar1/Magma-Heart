using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.Navigation;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.Core.UI;
using MagmaHeart.AI.Pathifinding;
using UnityEngine;

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
            
            GameUI uiInstance = Instantiate(m_uiPrefab);
            Player spawnedPlayer = SpawnPlayer(startRoom, uiInstance);

            uiInstance.Initialize(spawnedPlayer);
            uiInstance.HealthBar.gameObject.SetActive(true);

            if (m_sceneLoader.SavedData != null)
            {
                SaveData savedData = m_sceneLoader.SavedData;

                spawnedPlayer.Health.CurrentHealth = savedData.health;
            }

            GameStateMachine stateMachine = InitializeStateMachine(spawnedPlayer, uiInstance);
            uiInstance.RewardUI.Initialize(stateMachine);

            InitializeCombatSystem(startRoom, stateMachine);
        }

        private Player SpawnPlayer(RoomTileData startRoom, GameUI gameUI)
        {
            ActionUserInput actionUserInput = new ActionUserInput(m_userInput);
            CombatUserInput turnBasedUserInput = new CombatUserInput(m_userInput, m_grid);

            Player playerInstance = Instantiate(m_player, (Vector2)startRoom.WorldPosition, Quaternion.identity);
            playerInstance.Initialize(actionUserInput, turnBasedUserInput, gameUI);

            m_camera = Instantiate(m_cameraPrefab, new Vector3(startRoom.WorldPosition.x, startRoom.WorldPosition.y, -10), Quaternion.identity);
            m_camera.Initialize(playerInstance.transform, actionUserInput, turnBasedUserInput);

            playerInstance.Enable();

            return playerInstance;
        }

        private GameStateMachine InitializeStateMachine(Player player, GameUI ui)
        {
            List<IActionStateListener> actionListeners = new List<IActionStateListener>()
            { player };

            ActionState actionState = new ActionState(actionListeners);

            List<ICombatStateListener> combatStateListeners = new List<ICombatStateListener>()
            {
                player, m_camera, m_grid, ui
            };
            List<ICombatTurnSwitchListener> turnSwitchListeners = new List<ICombatTurnSwitchListener>()
            {
                m_camera.TurnBasedCameraBehaviour, ui
            };

            Spawner spawner = new Spawner(player, m_enemyPrefab, m_minDistanceFromPlayer);
            Battle battle = new Battle(player.TurnBasedPlayerBehaviour, spawner, turnSwitchListeners);
            CombatState combatState = new CombatState(battle, combatStateListeners);

            ArtifactDatabase database = new ArtifactDatabase();
            BattleReward battleReward = new BattleReward(database);
            // TODO: Unsubscribe somewhere
            battleReward.OnBattleRewardCalculated += ui.RewardUI.HandleOnBattleRewardCalculated;

            List<IRewardStateListener> rewardStateListeners = new List<IRewardStateListener>()
            {
                player, ui.RewardUI, battleReward
            };
            RewardState rewardState = new RewardState(rewardStateListeners);

            return new GameStateMachine(actionState, combatState, rewardState, battle);
        }

        private void InitializeCombatSystem(RoomTileData startRoom, GameStateMachine stateMachine)
        {
            RoomTileData bossRoom = m_location.GetFarthestRoomFrom(startRoom);

            foreach (RoomTileData roomTileData in m_location.Rooms)
            {
                if (roomTileData != startRoom)
                {
                    CombatTilemapRenderer renderer = Instantiate(m_combatTilemapRendererPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                    AStarGraph aStarGraph = AStarGraphBuilder.GenerateAStarGraph(roomTileData);
                    Room room = new Room(roomTileData, m_grid, renderer, aStarGraph);

                    if (roomTileData != bossRoom)
                    {
                        CombatAltar altarInstance = Instantiate(m_combatAltarPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                        altarInstance.Initialize(room, stateMachine);
                    }
                }
            }
        }
    }
}