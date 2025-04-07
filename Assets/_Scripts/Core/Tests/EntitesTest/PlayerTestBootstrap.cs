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
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CameraMovement m_cameraMovement;
        private TestLocationGenerator m_locationGenerator;
        private LocationRenderer m_locationRenderer;
        
        public void Awake()
        {
            m_locationGenerator = GetComponent<TestLocationGenerator>();
            m_locationRenderer = GetComponent<LocationRenderer>();

            Location location = m_locationGenerator.GenerateLocation();
            StartCoroutine(m_locationRenderer.DrawTiles(location.Tiles));

            Entity entityInstance = Instantiate(m_entityToSpawn, m_spawnPoint.position, Quaternion.identity);
            entityInstance.Initialize();
            m_healthBar.Initialize(entityInstance);
            m_cameraMovement.ObjectToTrack = entityInstance.transform;
        }
    }
}