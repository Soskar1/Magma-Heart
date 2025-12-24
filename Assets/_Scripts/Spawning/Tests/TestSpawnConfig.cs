using UnityEngine;

namespace MagmaHeart.Spawning.Tests
{
    internal class TestSpawnConfig : SpawnConfig
    {
        public bool Initialized { get; private set; }
        
        public TestSpawnConfig(GameObject prefab) : base(prefab) { }

        public override void Initialize(GameObject gameObject, SpawnContext spawnContext) => Initialized = true;
    }
}