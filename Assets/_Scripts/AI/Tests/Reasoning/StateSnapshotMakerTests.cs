using MagmaHeart.Collections;
using NUnit.Framework;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class StateSnapshotMakerTests
    {
        [Test]
        public void StateSnapshotMaker_CreateStateSnapshot_CreatesStateSnapshot()
        {
            Entity entity1 = new Entity(4, new Vector2(4, 4), false);
            Entity entity2 = new Entity(2, new Vector2(0, 1), true);
            Entity entity3 = new Entity(8, new Vector2(2, 4), false);
            CircularList<AIUnit> units = new CircularList<AIUnit> { entity1, entity2, entity3 };

            StateSnapshot snapshot = StateSnapshotMaker.CreateStateSnapshot(units);

            AssertEntity(entity1, 4, new Vector2(4, 4));
            AssertEntity(entity2, 2, new Vector2(0, 1));
            AssertEntity(entity3, 8, new Vector2(2, 4));

            void AssertEntity(Entity entity, float expectedHealth, Vector2 expectedPosition)
            {
                Assert.That(snapshot.StateProperties.ContainsKey(entity), Is.True);

                IsAliveProperty isAlive = snapshot.GetProperty<IsAliveProperty>(entity);
                Assert.That((bool)isAlive, Is.True);

                Health health = snapshot.GetProperty<Health>(entity);
                Assert.That(health.CurrentHealth, Is.EqualTo(expectedHealth));

                Position position = snapshot.GetProperty<Position>(entity);
                Assert.That((Vector2)position, Is.EqualTo(expectedPosition));
            }
        }
    }
}
