using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal class SimulationTests : CoreTests
    {
        internal (EntityModel, EntityModel) EnemyAndPlayerNearEachOther()
        {
            Vector2Int playerPosition = new Vector2Int(2, 3);
            Vector2Int enemyPosition = new Vector2Int(3, 3);
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(true).At(playerPosition.x, playerPosition.y)
                .AddEntity().IsPlayer(false).At(enemyPosition.x, enemyPosition.y)
                .Build();

            State.Room.TryGetUnit(playerPosition.ToVector2(), out EntityModel player);
            State.Room.TryGetUnit(enemyPosition.ToVector2(), out EntityModel enemy);

            return (player, enemy);
        }

        internal EntityModel BoardWithOneEntity()
        {
            Vector2Int entityPosition = new Vector2Int(3, 3);
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().At(entityPosition.x, entityPosition.y)
                .Build();

            State.Room.TryGetUnit(entityPosition.ToVector2(), out EntityModel entity);
            return entity;
        }

        [Test]
        public void ApplyDamageStateChange_OneExecution_AppliesDamageToTargetInSimulation()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(player.Id, 1);

            stateChange.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(player.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(4));
        }

        [Test]
        public void ApplyDamageStateChange_TwoExecutions_AppliesDamageToTargetInSimulation()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(player.Id, 2);

            stateChange.ApplyChangeToSimulation(simulation);
            stateChange.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(player.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(1));
        }

        [Test]
        public void ApplyDamageStateChange_DamageIsGreaterThanTargetsCurrentHealth_SetsTargetIsAliveToFalse()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(player.Id, 6);

            stateChange.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(player.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(0));
            Assert.That(simulatedPlayer.IsDisabled, Is.True);
        }

        [Test]
        public void UpdateEnergyStateChange_OneExecution_UpdatesEnergyForTargetInSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity.Id, 0, 3);

            energyChange.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(entity.Id, out EntityModel simulatedEntity);
            Assert.That(simulatedEntity.Energy.CurrentEnergy, Is.EqualTo(3));
        }

        [Test]
        public void UpdateEnergyStateChange_TwoExecutions_LastExecutionIsAppliedToSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity.Id, 0, 3);
            UpdateEnergyStateChange energyChange2 = new UpdateEnergyStateChange(entity.Id, 3, 4);

            energyChange.ApplyChangeToSimulation(simulation);
            energyChange2.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(entity.Id, out EntityModel simulatedEntity);
            Assert.That(simulatedEntity.Energy.CurrentEnergy, Is.EqualTo(4));
        }

        [Test]
        public void UpdateEnergyStateChange_NewEnergyValueIsAboveMaxEnergy_SetsEnergyValueToMaxEnergy()
        {
            EntityModel entity = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            int energyToSet = entity.Energy.MaxEnergy + 1;
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity.Id, entity.Energy.CurrentEnergy, energyToSet);

            energyChange.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(entity.Id, out EntityModel simulatedEntity);
            Assert.That(simulatedEntity.Energy.CurrentEnergy, Is.EqualTo(entity.Energy.MaxEnergy));
        }

        [Test]
        public void UpdateEnergyStateChange_NewEnergyValueIsNegative_SetsEnergyValueToZero()
        {
            EntityModel entity = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity.Id, entity.Energy.CurrentEnergy, -1);

            energyChange.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(entity);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(0));
        }

        [Test]
        public void MoveEntityStateChange_UpdatesBoardNodeTypesAndMovesEntity()
        {
            EntityModel entity = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);

            Vector2Int initialPosition = entity.GetCurrentTilePosition().ToVector2Int();
            Vector2Int endPosition = new Vector2Int(5, 5);
            MoveEntityStateChange moveChange = new MoveEntityStateChange(entity.Id, new List<Vector2>() { initialPosition, endPosition });

            moveChange.ApplyChangeToSimulation(simulation);

            Assert.That(simulation.Board.TryGetUnit(endPosition, out EntityModel simulatedEntity), Is.True);
            Assert.That(simulatedEntity.CurrentTilePosition.ToVector2Int(), Is.EqualTo(endPosition));
            Assert.That(simulation.Board.TryGetUnit(initialPosition, out _), Is.False);
            Assert.That(simulation.Board.GetNodeType(initialPosition), Is.EqualTo(BoardNodeType.Walkable));
            Assert.That(simulation.Board.GetNodeType(endPosition), Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void AttackStateChange_MeleeAttack_CreatesApplyDamageStateChange()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            AttackStateChange stateChange = new AttackStateChange(enemy.Id, player.Id, 2, AttackType.Melee);

            stateChange.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(player.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(3));
        }

        [Test]
        public void AttackStateChange_RangedAttack_CreatesApplyDamageStateChange()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(State.Board);
            AttackStateChange stateChange = new AttackStateChange(enemy.Id, player.Id, 2, AttackType.Ranged);

            stateChange.ApplyChangeToSimulation(simulation);

            simulation.Board.TryGetUnit(player.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(3));
        }
    }
}