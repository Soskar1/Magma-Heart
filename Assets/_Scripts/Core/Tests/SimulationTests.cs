using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class SimulationTests : CoreTests
    {
        internal (EntityModel, EntityModel) EnemyAndPlayerNearEachOther()
        {
            Vector2Int playerPosition = new Vector2Int(2, 3);
            Vector2Int enemyPosition = new Vector2Int(3, 3);
            AIScenarioBuilder.Create(World, TestDatabase)
                .AddEntity().IsPlayer(true).WithData(EntityDatabase.Warrior).At(playerPosition.x, playerPosition.y)
                .AddEntity().IsPlayer(false).WithData(EntityDatabase.SkeletonWarrior).At(enemyPosition.x, enemyPosition.y)
                .Build();

            Board.TryGetUnit(playerPosition, out EntityModel player);
            Board.TryGetUnit(enemyPosition, out EntityModel enemy);

            return (player, enemy);
        }

        internal EntityModel BoardWithOneEntity()
        {
            Vector2Int entityPosition = new Vector2Int(3, 3);
            AIScenarioBuilder.Create(World, TestDatabase)
                .AddEntity().WithData(EntityDatabase.Warrior).At(entityPosition.x, entityPosition.y)
                .Build();

            Board.TryGetUnit(entityPosition, out EntityModel entity);
            return entity;
        }

        [Test]
        public void ApplyDamageEffect_OneExecution_AppliesDamageToTargetInSimulation()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther(); 
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();

            dispatcher.Apply(simulation, new DamageEffect(enemy.Id, player.Id, 6, TestDatabase.Health));

            Assert.That(player.Health.CurrentHealth, Is.EqualTo(4));
        }

        [Test]
        public void ApplyDamageEffect_Undo_RevertsDamageToTargetInSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();

            simulation.SaveCheckpoint();
            dispatcher.Apply(simulation, new DamageEffect(entity.Id, entity.Id, 2, TestDatabase.Health));
            simulation.RestoreCheckpoint();

            Assert.That(entity.Health.CurrentHealth, Is.EqualTo(10));
        }

        [Test]
        public void ApplyDamageEffect_UndoWithLowHealth_RevertsDamageToTargetInSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            entity.Health.CurrentHealth = 1;
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();

            simulation.SaveCheckpoint();
            dispatcher.Apply(simulation, new DamageEffect(entity.Id, entity.Id, 3, TestDatabase.Health));
            simulation.RestoreCheckpoint();

            Assert.That(entity.Health.CurrentHealth, Is.EqualTo(1));
        }

        [Test]
        public void RestoreEnergyEffect_OneExecution_IncreasesEnergyForTargetInSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();

            dispatcher.Apply(simulation, new RestoreParameterEffect(entity.Id, TestDatabase.Energy, 3));

            Assert.That(entity.Energy.CurrentEnergy, Is.EqualTo(3));
        }

        [Test]
        public void RestoreEnergyEffect_RestorationExceedsMaxEnergy_ClampsToMaxEnergy()
        {
            EntityModel entity = BoardWithOneEntity();
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();

            dispatcher.Apply(simulation, new RestoreParameterEffect(entity.Id, TestDatabase.Energy, entity.Energy.MaxEnergy + 1));

            Assert.That(entity.Energy.CurrentEnergy, Is.EqualTo(entity.Energy.MaxEnergy));
        }

        [Test]
        public void SpendEnergyEffect_SpendMoreThanAvailable_ClampsToZero()
        {
            EntityModel entity = BoardWithOneEntity();
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();

            dispatcher.Apply(simulation, new SpendResourceEffect(entity.Id, TestDatabase.Energy, 1));

            Assert.That(entity.Energy.CurrentEnergy, Is.EqualTo(0));
        }

        [Test]
        public void RestoreEnergyEffect_Undo_ReturnsInitialValue()
        {
            EntityModel entity = BoardWithOneEntity();
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();

            simulation.SaveCheckpoint();
            dispatcher.Apply(simulation, new RestoreParameterEffect(entity.Id, TestDatabase.Energy, 4));
            simulation.RestoreCheckpoint();

            Assert.That(entity.Energy.CurrentEnergy, Is.EqualTo(0));
        }

        [Test]
        public void MoveEffect_UpdatesBoardNodeTypesAndMovesEntity()
        {
            EntityModel entity = BoardWithOneEntity();
            Vector2Int initialPosition = entity.TilePosition.ToVector2Int();
            Vector2Int endPosition = new Vector2Int(5, 5);
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();
            List<Vector3> path = new List<Vector3> { new Vector3(initialPosition.x, initialPosition.y), new Vector3(endPosition.x, endPosition.y) };

            dispatcher.Apply(simulation, new MoveEffect(entity.Id, path));

            Assert.That(Board.TryGetUnit(endPosition, out EntityModel _), Is.True);
            Assert.That(Board.TryGetUnit(initialPosition, out _), Is.False);
            Assert.That(Board.GetNodeType(initialPosition), Is.EqualTo(BoardNodeType.Walkable));
            Assert.That(Board.GetNodeType(endPosition), Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void MoveEffect_Undo_RevertsNodeTypesAndRestoresEntityPosition()
        {
            EntityModel entity = BoardWithOneEntity();
            Vector2Int initialPosition = entity.TilePosition.ToVector2Int();
            Vector2Int endPosition = new Vector2Int(5, 5);
            WorldSimulation simulation = new WorldSimulation(Board);
            EffectDispatcher dispatcher = CreateDispatcher();
            List<Vector3> path = new List<Vector3> { new Vector3(initialPosition.x, initialPosition.y), new Vector3(endPosition.x, endPosition.y) };

            simulation.SaveCheckpoint();
            dispatcher.Apply(simulation, new MoveEffect(entity.Id, path));
            simulation.RestoreCheckpoint();

            Assert.That(Board.TryGetUnit(initialPosition, out EntityModel _), Is.True);
            Assert.That(Board.TryGetUnit(endPosition, out _), Is.False);
            Assert.That(Board.GetNodeType(initialPosition), Is.EqualTo(BoardNodeType.Obstacle));
            Assert.That(Board.GetNodeType(endPosition), Is.EqualTo(BoardNodeType.Walkable));
        }
    }
}