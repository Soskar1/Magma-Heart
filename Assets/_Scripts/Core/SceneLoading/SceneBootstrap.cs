using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;
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
        [SerializeField] private Room m_roomPrefab;
        [SerializeField] private GameUI m_uiPrefab;
        [SerializeField] private GameGrid m_gridPrefab;
        [SerializeField] private Teleporter m_teleporterPrefab;

        [Header("Combat Event")]
        [SerializeField] private Spawner m_spawnerPrefab;
        [SerializeField] private List<Artifact> m_artifactPrefabs;

        [Header("CombatData")]
        [SerializeField] private CombatData m_monsterRooms;
        [SerializeField] private CombatData m_bossRooms;

        private SceneLoader m_sceneLoader;
        private Location m_location;
        private LocationRenderer m_renderer;
        private GameGrid m_grid;
        private List<Artifact> m_artifactsToApply;

        public void Initialize(SceneLoader sceneLoader, List<Artifact> artifactsToApply)
        {
            m_sceneLoader = sceneLoader;
            m_artifactsToApply = artifactsToApply;
        }

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

            if (m_artifactsToApply != null && spawnedEntity.TryGetComponent(out ArtifactApplier applier))
                applier.ApplyArtifacts(m_artifactsToApply);

            Spawner spawnerInstance = Instantiate(m_spawnerPrefab);
            spawnerInstance.Initialize(spawnedEntity);

            CombatEvent combatEvent = new CombatEvent(spawnerInstance);

            m_monsterRooms.OnCombatEnded += OpenCorridors;
            m_monsterRooms.OnCombatEnded += SpawnArtifact;
            m_bossRooms.OnCombatEnded += OpenCorridors;
            m_bossRooms.OnCombatEnded += ActivateTeleporter;

            foreach (RoomTileData roomTileData in m_location.Rooms)
            {
                if (roomTileData != startRoom)
                {
                    Room roomInstance = Instantiate(m_roomPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                    List<Corridor> adjacentCorridors = m_location.Corridors.FindAll(c => c.Entrance1.RoomTileData == roomTileData || c.Entrance2.RoomTileData == roomTileData);

                    if (roomTileData != bossRoom)
                        roomInstance.Initialize(roomTileData, adjacentCorridors, m_monsterRooms);
                    else
                        roomInstance.Initialize(roomTileData, adjacentCorridors, m_bossRooms);
                    
                    roomInstance.playerEnteredRoom += combatEvent.Start;
                    roomInstance.playerEnteredRoom += (room) => m_grid.Corridors.gameObject.SetActive(true);
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
        
        private void OpenCorridors(Room room)
        {
            m_grid.Corridors.gameObject.SetActive(false);
        }

        private void SpawnArtifact(Room room)
        {
            Artifact prefab = m_artifactPrefabs[Random.Range(0, m_artifactPrefabs.Count)];
            Instantiate(prefab, room.WorldPosition.ToVector3(), Quaternion.identity);
        }

        private void ActivateTeleporter(Room room)
        {
            Teleporter teleporterInstance = Instantiate(m_teleporterPrefab, room.WorldPosition.ToVector2(), Quaternion.identity);
            teleporterInstance.Initialize(m_sceneLoader);
        }
    }
}