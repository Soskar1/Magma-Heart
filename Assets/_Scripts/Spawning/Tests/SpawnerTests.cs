using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace MagmaHeart.Spawning.Tests
{
    internal class SpawnerTests
    {
        [Test]
        public void Spawn_ReturnsInitializedGameObject()
        {
            GameObject testPrefab = new GameObject("TestPrefab");
            TestSpawnConfig config = new TestSpawnConfig(testPrefab);
            FakeInstantiator instantiator = new FakeInstantiator();
            Spawner spawner = new Spawner(
                new Dictionary<GameObject, ISpawnConfig>
                {
                    { testPrefab, config }
                },
                instantiator);

            SpawnContext context = new SpawnContext(new Vector2(4, 3));

            GameObject instance = spawner.Spawn(testPrefab, context);

            Assert.IsNotNull(instance);
            Assert.IsTrue(config.Initialized);
            Assert.AreEqual(context.Position, instantiator.LastSpawnPoint);
            Assert.AreEqual(config.Prefab, instantiator.LastPrefab);
        }
    }
}