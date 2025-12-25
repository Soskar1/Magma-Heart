using UnityEngine;

namespace MagmaHeart.Spawning.Tests
{
    internal class FakeInstantiator : IInstantiator
    {
        public GameObject LastPrefab { get; private set; }
        public Vector2 LastSpawnPoint { get; private set; }

        public GameObject Instantiate(GameObject prefab, Vector2 position)
        {
            LastPrefab = prefab;
            LastSpawnPoint = position;
            return new GameObject("SpawnedObject");
        }
    }
}

