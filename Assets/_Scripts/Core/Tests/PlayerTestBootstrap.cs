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
        [SerializeField] private Entity m_entityToSpawn;

        [SerializeField] private EnemyBehaviour m_enemyToSpawn;
        [SerializeField] private Transform m_enemySpawnPoint;
        
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CameraMovement m_cameraMovement;
        [SerializeField] private LocationGenerator m_locationGenerator;
        private LocationRenderer m_locationRenderer;
        private Location m_location;

        [Header("A* Test")]
        [SerializeField] private Transform m_start;
        [SerializeField] private Transform m_end;
        private AStarNavigation m_navigation;
        
        public void Awake() => m_locationRenderer = GetComponent<LocationRenderer>();

        public async void Start()
        {
            m_location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            StartCoroutine(m_locationRenderer.DrawTiles(m_location.Tiles));

            Entity entityInstance = Instantiate(m_entityToSpawn, m_spawnPoint.position, Quaternion.identity);
            entityInstance.Initialize();
            m_healthBar.Initialize(entityInstance);
            m_cameraMovement.ObjectToTrack = entityInstance.transform;

            EnemyBehaviour enemyInstance = Instantiate(m_enemyToSpawn, m_enemySpawnPoint.position, Quaternion.identity);
            enemyInstance.Initialize(entityInstance, m_location.Rooms[0]);
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
            Debug.Log($"Iterations: {m_navigation.iterations}");
            Debug.Log("======");
        }
    }
}