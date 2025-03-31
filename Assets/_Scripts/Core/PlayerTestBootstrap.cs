using MagmaHeart.Core.Entities;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class PlayerTestBootstrap : MonoBehaviour
    {
        [SerializeField] private Transform m_spawnPoint;
        [SerializeField] private Entity m_entityToSpawn;
        [SerializeField] private HealthBar m_healthBar;
        [SerializeField] private CameraMovement m_cameraMovement;
        
        public void Awake()
        {
            Entity entityInstance = Instantiate(m_entityToSpawn, m_spawnPoint.position, Quaternion.identity);
            entityInstance.Initialize();
            m_healthBar.Initialize(entityInstance);
            m_cameraMovement.ObjectToTrack = entityInstance.transform;
        }
    }
}