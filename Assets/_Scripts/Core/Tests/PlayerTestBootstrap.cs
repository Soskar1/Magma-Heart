using System.Diagnostics;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.UI;
using UnityEngine;
using Debug=UnityEngine.Debug;

namespace MagmaHeart.Core.Tests
{
    public class PlayerTestBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform m_spawnPoint;
        [SerializeField] private PlayerBehaviour m_player;

        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CameraMovement m_cameraMovement;
        [SerializeField] private LocationGenerator m_locationGenerator;
        private LocationRenderer m_locationRenderer;
        private Location m_location;

        [Header("A* Test")]
        [SerializeField] private Transform m_start;
        [SerializeField] private Transform m_end;
        private AStarNavigation m_navigation;

        [Header("Spawner")]
        [SerializeField] private Spawner m_spawner;
        
        public void Awake() => m_locationRenderer = GetComponent<LocationRenderer>();

        public async void Start()
        {
            m_location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            StartCoroutine(m_locationRenderer.DrawTiles(m_location.Tiles));

            PlayerBehaviour playerInstance = Instantiate(m_player, m_spawnPoint.position, Quaternion.identity);
            playerInstance.Initialize();
            m_healthBar.Initialize(playerInstance.ControllingEntity);
            m_cameraMovement.ObjectToTrack = playerInstance.transform;
            playerInstance.Enable();

            m_spawner.Initialize(playerInstance.ControllingEntity);
            m_spawner.SetRoomData(m_location.Rooms[0]);

            m_navigation = new AStarNavigation(m_location.Rooms[0]);
        }

        public void AStarTest()
        {
            var path = m_navigation.ConstructPath(m_start.position.ToVector2Int(), m_end.position.ToVector2Int());

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Debug.Log("Path:");
            foreach (var point in path) {
                Debug.Log(point);
            }
            stopwatch.Stop();
            Debug.Log($"Elapsed time {stopwatch.ElapsedMilliseconds}ms");
            Debug.Log("======");
        }

        public void SpawnerTest() => m_spawner.SpawnWave();
    }
}