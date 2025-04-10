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
        [SerializeField] private PlayerBehaviour m_entityToSpawn;

        [SerializeField] private EnemyMeleeBehaviour m_enemyToSpawn;
        [SerializeField] private Transform m_enemySpawnPoint;
        [SerializeField] private Transform m_secondEnemySpawnPoint;
        
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

            PlayerBehaviour playerInstance = Instantiate(m_entityToSpawn, m_spawnPoint.position, Quaternion.identity);
            playerInstance.Initialize();
            m_healthBar.Initialize(playerInstance.ControllingEntity);
            m_cameraMovement.ObjectToTrack = playerInstance.transform;
            playerInstance.Enable();

            EnemyMeleeBehaviour enemyInstance = Instantiate(m_enemyToSpawn, m_enemySpawnPoint.position, Quaternion.identity);
            enemyInstance.Initialize(playerInstance.ControllingEntity, m_location.Rooms[0]);
            enemyInstance.Enable();

            EnemyMeleeBehaviour enemyInstance2 = Instantiate(m_enemyToSpawn, m_secondEnemySpawnPoint.position, Quaternion.identity);
            enemyInstance2.Initialize(playerInstance.ControllingEntity, m_location.Rooms[0]);
            enemyInstance2.Enable();

            Collider2D enemy1Collider = enemyInstance.GetComponent<Collider2D>();
            Collider2D enemy2Collider = enemyInstance2.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(enemy1Collider, enemyInstance2.AttackHitCollider);
            Physics2D.IgnoreCollision(enemyInstance.AttackHitCollider, enemy2Collider);

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
    }
}