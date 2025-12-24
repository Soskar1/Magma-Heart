using UnityEngine;

namespace MagmaHeart.Spawning.Tests
{
    internal class TestSpawnConfig : ISpawnConfig
    {
        private GameObject m_prefab;

        public GameObject Prefab => m_prefab;
        public bool Initialized { get; private set; }

        public TestSpawnConfig(GameObject prefab) => m_prefab = prefab;

        public void Initialize(GameObject gameObject, SpawnContext spawnContext)
        {
            Initialized = true;
        }
    }
}