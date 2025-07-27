using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.SceneLoading
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour m_player;
        [SerializeField] private CameraMovement m_camera;
        [SerializeField] private LocationGenerator m_locationGeneratorPrefab;
        [SerializeField] private GameUI m_uiPrefab;
        [SerializeField] private GameGrid m_gridPrefab;
        [SerializeField] private Teleporter m_teleporterPrefab;
        [SerializeField] private CombatAltar m_combatAltarPrefab;

        private SceneLoader m_sceneLoader;
        private Location m_location;
        private LocationRenderer m_renderer;
        private GameGrid m_grid;

        public void Initialize(SceneLoader sceneLoader) => m_sceneLoader = sceneLoader;

        public async void BootScene()
        {
            m_grid = Instantiate(m_gridPrefab);

            LocationGenerator locationGeneratorInstance = Instantiate(m_locationGeneratorPrefab);
            m_renderer = locationGeneratorInstance.GetComponent<LocationRenderer>();

            m_renderer.RenderedAllTiles += SpawnEntities;

            m_location = await locationGeneratorInstance.GenerateLocation(Vector2Int.zero);
            m_renderer.AddTilesToDraw(m_location.FloorTiles, m_grid.Floor, m_grid.FloorTile);
            m_renderer.AddTilesToDraw(m_location.WallTiles, m_grid.Walls, m_grid.WallTile);
            m_renderer.AddTilesToDraw(m_location.CorridorEntranceTiles, m_grid.Corridors, m_grid.WallTile);
            m_renderer.DrawTiles();
        }

        private void SpawnEntities()
        {
            m_renderer.RenderedAllTiles -= SpawnEntities;

            RoomTileData startRoom = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];
            RoomTileData bossRoom = m_location.GetFarthestRoomFrom(startRoom);

            Entity spawnedEntity = SpawnPlayer(startRoom);

            GameUI uiInstance = Instantiate(m_uiPrefab);
            uiInstance.HealthBar.Initialize(spawnedEntity);
            uiInstance.HealthBar.gameObject.SetActive(true);

            if (m_sceneLoader.SavedData != null)
            {
                SaveData savedData = m_sceneLoader.SavedData;

                if (spawnedEntity.TryGetComponent(out ArtifactApplier applier))
                    applier.ApplyArtifacts(savedData.ObtainedArtifacts);

                spawnedEntity.Health.SetCurrentHealth(savedData.health);
            }

            foreach (RoomTileData room in m_location.Rooms)
            {
                if (room != startRoom && room != bossRoom)
                {
                    Instantiate(m_combatAltarPrefab, room.WorldPosition.ToVector3(), Quaternion.identity);
                }
            }
        }

        private Entity SpawnPlayer(RoomTileData startRoom)
        {
            PlayerBehaviour playerInstance = Instantiate(m_player, (Vector2)startRoom.WorldPosition, Quaternion.identity);
            playerInstance.Initialize();
            
            CameraMovement cameraInstance = Instantiate(m_camera, new Vector3(startRoom.WorldPosition.x, startRoom.WorldPosition.y, -10), Quaternion.identity);
            cameraInstance.ObjectToTrack = playerInstance.transform;

            playerInstance.Enable();

            return playerInstance.ControllingEntity;
        }
    }
}