using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class DungeonBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour m_player;
        [SerializeField] private CameraMovement m_camera;
        [SerializeField] private LocationGenerator m_locationGenerator;
        [SerializeField] private LocationRenderer m_renderer;
        [SerializeField] private Room m_roomPrefab;
        [SerializeField] private Spawner m_spawnerPrefab;

        private Location m_location;

        private void Awake() => m_renderer.RenderedAllTiles += SpawnEntities;

        private void Start() => BootScene();

        private async void BootScene()
        {
            m_location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            StartCoroutine(m_renderer.DrawTiles(m_location.Tiles));
        }

        private void SpawnEntities()
        {
            m_renderer.RenderedAllTiles -= SpawnEntities;

            RoomTileData startRoom = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];
            Entity spawnedEntity = SpawnPlayer(startRoom);

            Spawner spawner = Instantiate(m_spawnerPrefab);
            spawner.Initialize(spawnedEntity);

            foreach (RoomTileData roomTileData in m_location.Rooms)
            {
                if (roomTileData != startRoom)
                {
                    Room roomInstance = Instantiate(m_roomPrefab, roomTileData.WorldPosition.ToVector3(), Quaternion.identity);
                    List<Corridor> adjacentCorridors = m_location.Corridors.FindAll(c => c.Room1 == roomTileData || c.Room2 == roomTileData);
                    roomInstance.Initialize(roomTileData, adjacentCorridors);

                    roomInstance.playerEnteredRoom += spawner.SetRoomTileData;
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