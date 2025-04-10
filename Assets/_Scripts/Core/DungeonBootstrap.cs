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

        private Location m_location;

        private void Awake() => m_renderer.RenderedAllTiles += SpawnPlayer;

        private void Start() => BootScene();

        private async void BootScene()
        {
            m_location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            StartCoroutine(m_renderer.DrawTiles(m_location.Tiles));
        }

        private void SpawnPlayer()
        {
            RoomData roomData = m_location.Rooms[Random.Range(0, m_location.Rooms.Count)];
            PlayerBehaviour playerInstance = Instantiate(m_player, (Vector2)roomData.WorldPosition, Quaternion.identity);
            m_renderer.RenderedAllTiles -= SpawnPlayer;

            CameraMovement cameraInstance = Instantiate(m_camera, new Vector3(roomData.WorldPosition.x, roomData.WorldPosition.y, -10), Quaternion.identity);
            cameraInstance.ObjectToTrack = playerInstance.transform;
        }
    }
}