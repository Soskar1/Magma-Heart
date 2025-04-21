using System.Collections.Generic;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
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

        [Header("Combat Event")]
        [SerializeField] private Spawner m_spawnerPrefab;
        [SerializeField] private int m_amountOfWaves;
        [SerializeField] private List<Artifact> m_artifactPrefabs;

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
            Entity spawnedEntity = SpawnPlayer(startRoom);

            Spawner spawner = Instantiate(m_spawnerPrefab);
            spawner.Initialize(spawnedEntity);

            CombatEvent combatEvent = new CombatEvent(spawner, m_amountOfWaves, m_artifactPrefabs);
            combatEvent.OnCombatEventEnded += EndCombatEvent;

            foreach (RoomTileData roomTileData in m_location.Rooms)
            {
                if (roomTileData != startRoom)
                {
                    Room roomInstance = Instantiate(m_roomPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                    List<Corridor> adjacentCorridors = m_location.Corridors.FindAll(c => c.Entrance1.RoomTileData == roomTileData || c.Entrance2.RoomTileData == roomTileData);
                    roomInstance.Initialize(roomTileData, adjacentCorridors);

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
            Debug.Log("Combat event ended");
        }
    }
}