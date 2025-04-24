using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core
{
    public class DungeonBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour m_player;
        [SerializeField] private CameraMovement m_camera;
        [SerializeField] private LocationGenerator m_locationGenerator;
        [SerializeField] private LocationRenderer m_renderer;
        [SerializeField] private Room m_roomPrefab;
        [SerializeField] private HealthBar m_healthBar;

        [Header("Combat Event")]
        [SerializeField] private Spawner m_spawner;
        [SerializeField] private int m_amountOfWaves;
        [SerializeField] private List<Artifact> m_artifactPrefabs;

        [Header("Enemies")]
        [SerializeField] private List<RoomEnemy> m_monsterRoomEnemies;
        [SerializeField] private List<RoomEnemy> m_bossRoomEnemies;

        [Header("GFX")]
        [SerializeField] private Tilemap m_floor;
        [SerializeField] private Tilemap m_walls;
        [SerializeField] private Tilemap m_corridorEntrances;
        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;

        private Location m_location;

        private void Awake() => m_renderer.RenderedAllTiles += SpawnEntities;

        private void Start() => BootScene();

        private async void BootScene()
        {
            m_location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            m_renderer.AddTilesToDraw(m_location.FloorTiles, m_floor, m_floorTile);
            m_renderer.AddTilesToDraw(m_location.WallTiles, m_walls, m_wallTile);
            m_renderer.AddTilesToDraw(m_location.CorridorEntranceTiles, m_corridorEntrances, m_wallTile);
            m_renderer.DrawTiles();
        }

        private void SpawnEntities()
        {
            m_renderer.RenderedAllTiles -= SpawnEntities;

            RoomTileData startRoom = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];
            RoomTileData bossRoom = m_location.GetFarthestRoomFrom(startRoom);

            Entity spawnedEntity = SpawnPlayer(startRoom);
            m_healthBar.Initialize(spawnedEntity);
            m_healthBar.gameObject.SetActive(true);

            m_spawner.Initialize(spawnedEntity);

            CombatEvent combatEvent = new CombatEvent(m_spawner, m_amountOfWaves, m_artifactPrefabs);
            combatEvent.OnCombatEventEnded += EndCombatEvent;

            foreach (RoomTileData roomTileData in m_location.Rooms)
            {
                if (roomTileData != startRoom)
                {
                    Room roomInstance = Instantiate(m_roomPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                    List<Corridor> adjacentCorridors = m_location.Corridors.FindAll(c => c.Entrance1.RoomTileData == roomTileData || c.Entrance2.RoomTileData == roomTileData);

                    if (roomTileData != bossRoom)
                        roomInstance.Initialize(roomTileData, adjacentCorridors, m_monsterRoomEnemies);
                    else
                        roomInstance.Initialize(roomTileData, adjacentCorridors, m_bossRoomEnemies);

                    roomInstance.playerEnteredRoom += combatEvent.Start;
                    roomInstance.playerEnteredRoom += (room) => m_corridorEntrances.gameObject.SetActive(true);
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
        
        private void EndCombatEvent()
        {
            m_corridorEntrances.gameObject.SetActive(false);
        }
    }
}