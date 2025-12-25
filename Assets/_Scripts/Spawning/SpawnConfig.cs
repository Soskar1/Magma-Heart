using UnityEngine;

namespace MagmaHeart.Spawning
{
    public abstract class SpawnConfig
    {
        public GameObject Prefab { get; private set; }

        public SpawnConfig(GameObject prefab) => Prefab = prefab;

        public abstract void Initialize(GameObject instance, SpawnContext spawnContext);
    }
}