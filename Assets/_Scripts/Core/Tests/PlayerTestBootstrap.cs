using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.UI;
using UnityEngine;

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
        
        public void Awake() => m_locationRenderer = GetComponent<LocationRenderer>();

        public async void Start()
        {
            Location location = await m_locationGenerator.GenerateLocation(Vector2Int.zero);
            StartCoroutine(m_locationRenderer.DrawTiles(location.Tiles));

            Entity entityInstance = Instantiate(m_entityToSpawn, m_spawnPoint.position, Quaternion.identity);
            entityInstance.Initialize();
            m_healthBar.Initialize(entityInstance);
            m_cameraMovement.ObjectToTrack = entityInstance.transform;

            EnemyBehaviour enemyInstance = Instantiate(m_enemyToSpawn, m_enemySpawnPoint.position, Quaternion.identity);
            enemyInstance.Initialize(entityInstance, location.Rooms[0]);
        }
    }
}