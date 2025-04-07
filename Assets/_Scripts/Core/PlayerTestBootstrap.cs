using MagmaHeart.Core.Dungeon;
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
        private TestRoomGenerator m_testRoomGenerator;
        private LocationRenderer m_locationRenderer;
        
        public void Awake()
        {
            m_testRoomGenerator = GetComponent<TestRoomGenerator>();
            m_locationRenderer = GetComponent<LocationRenderer>();

            RoomData roomData = m_testRoomGenerator.CreateRoom();
            StartCoroutine(m_locationRenderer.DrawTiles(roomData.GetTiles()));

            Entity entityInstance = Instantiate(m_entityToSpawn, m_spawnPoint.position, Quaternion.identity);
            entityInstance.Initialize();
            m_healthBar.Initialize(entityInstance);
            m_cameraMovement.ObjectToTrack = entityInstance.transform;
        }
    }
}