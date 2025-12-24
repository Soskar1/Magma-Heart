using UnityEngine;

namespace MagmaHeart.Spawning
{
    public interface ISpawnConfig
    {
        public GameObject Prefab { get; }
        public void Initialize(GameObject gameObject, SpawnContext spawnContext);
    }
}