using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private CameraMovement m_camera;
        [SerializeField] private LocationGenerator m_locationGeneratorPrefab;
        [SerializeField] private GameUI m_uiPrefab;
        [SerializeField] private GameGrid m_gridPrefab;
        [SerializeField] private Teleporter m_teleporterPrefab;
        [SerializeField] private CombatAltar m_combatAltarPrefab;
        [SerializeField] private Room m_roomPrefab;

        [Header("Spawner Settings")]
        [SerializeField] private Enemy m_enemyPrefab;
        [SerializeField] private float m_minDistanceFromPlayer;

        private SceneLoader m_sceneLoader;
        private Location m_location;
        private LocationRenderer m_renderer;
        private CombatStateSwitcher m_combatStateSwitcher;
        private UserInput m_userInput;
        private GameGrid m_grid;

        public void Initialize(SceneLoader sceneLoader) => m_sceneLoader = sceneLoader;

        public async void BootScene()
        {
            m_userInput = new UserInput();

            m_grid = Instantiate(m_gridPrefab);

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
            Player spawnedPlayer = SpawnPlayer(startRoom, uiInstance.CombatRelatedUI);

            uiInstance.Initialize(spawnedPlayer);
            uiInstance.HealthBar.gameObject.SetActive(true);

            if (m_sceneLoader.SavedData != null)
            {
                SaveData savedData = m_sceneLoader.SavedData;

                if (spawnedPlayer.TryGetComponent(out ArtifactApplier applier))
                    applier.ApplyArtifacts(savedData.ObtainedArtifacts);

                spawnedPlayer.Health.SetCurrentHealth(savedData.health);
            }

            InitializeCombatSystem(spawnedPlayer, startRoom);
        }

        private Player SpawnPlayer(RoomTileData startRoom, List<IDisplayable> combatUI)
        {
            Player playerInstance = Instantiate(m_player, (Vector2)startRoom.WorldPosition, Quaternion.identity);
            playerInstance.Initialize(m_userInput, combatUI);
            
            CameraMovement cameraInstance = Instantiate(m_camera, new Vector3(startRoom.WorldPosition.x, startRoom.WorldPosition.y, -10), Quaternion.identity);
            cameraInstance.ObjectToTrack = playerInstance.transform;

            playerInstance.Enable();

            return playerInstance;
        }

        private void InitializeCombatSystem(Player player, RoomTileData startRoom)
        {
            RoomTileData bossRoom = m_location.GetFarthestRoomFrom(startRoom);

            Spawner spawner = new Spawner(player, m_enemyPrefab, m_minDistanceFromPlayer);
            m_combatStateSwitcher = new CombatStateSwitcher(m_grid.Corridors, player, spawner);

            foreach (RoomTileData roomTileData in m_location.Rooms)
            {
                if (roomTileData != startRoom)
                {
                    Room roomInstance = Instantiate(m_roomPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                    roomInstance.Initialize(roomTileData, m_grid);

                    if (roomTileData != bossRoom)
                    {
                        CombatAltar altarInstance = Instantiate(m_combatAltarPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                        altarInstance.Initialize(roomInstance, m_combatStateSwitcher);
                    }
                }
            }
        }
    }
}