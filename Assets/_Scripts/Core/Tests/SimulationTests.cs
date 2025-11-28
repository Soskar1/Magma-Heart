using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal class SimulationTests : CoreTests
    {
        [Test]
        public void ApplyDamageStateChange_OneExecution_AppliesDamageToTargetInSimulation()
        {
            EntityModel player = (EntityModel)AddEntity(new Vector3Int(2, 3), true).Model;
            EntityModel enemy = (EntityModel)AddEntity(new Vector3Int(3, 3), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(enemy, player, 1);

            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(4));
        }

        [Test]
        public void ApplyDamageStateChange_TwoExecutions_AppliesDamageToTargetInSimulation()
        {
            EntityModel player = (EntityModel)AddEntity(new Vector3Int(2, 3), true).Model;
            EntityModel enemy = (EntityModel)AddEntity(new Vector3Int(3, 3), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(enemy, player, 2);

            stateChange.ApplyChangeToSimulation(simulation);
            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(1));
        }

        [Test]
        public void ApplyDamageStateChange_DamageIsGreaterThanTargetsCurrentHealth_SetsTargetIsAliveToFalse()
        {
            EntityModel player = (EntityModel)AddEntity(new Vector3Int(2, 3), true).Model;
            EntityModel enemy = (EntityModel)AddEntity(new Vector3Int(3, 3), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(enemy, player, 6);

            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(player);
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(-1));
            Assert.That(isAlive.IsAlive, Is.True);
        }

        [Test]
        public void UpdateEnergyStateChange_OneExecution_UpdatesEnergyForTargetInSimulation()
        {
            EntityModel enemy = (EntityModel)AddEntity(new Vector3Int(3, 3), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(enemy, 3);

            energyChange.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(enemy);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(3));
        }

        [Test]
        public void UpdateEnergyStateChange_TwoExecutions_LastExecutionIsAppliedToSimulation()
        {
            EntityModel enemy = (EntityModel)AddEntity(new Vector3Int(3, 3), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(enemy, 3);
            UpdateEnergyStateChange energyChange2 = new UpdateEnergyStateChange(enemy, 4);

            energyChange.ApplyChangeToSimulation(simulation);
            energyChange2.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(enemy);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(4));
        }

        [Test]
        public void UpdateEnergyStateChange_NewEnergyValueIsAboveMaxEnergy_SetsEnergyValueToMaxEnergy()
        {
            EntityModel enemy = (EntityModel)AddEntity(new Vector3Int(3, 3), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            int energyToSet = enemy.Energy.MaxEnergy + 1;
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(enemy, energyToSet);

            energyChange.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(enemy);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(enemy.Energy.MaxEnergy));
        }

        [Test]
        public void UpdateEnergyStateChange_NewEnergyValueIsNegative_SetsEnergyValueToZero()
        {
            EntityModel enemy = (EntityModel)AddEntity(new Vector3Int(3, 3), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(enemy, -1);

            energyChange.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(enemy);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(0));
        }

        [Test]
        public void MoveEntityStateChange_UpdatesBoardNodeTypesAndMovesEntity()
        {
            Vector2 initialPosition = new Vector2Int(3, 3);
            Vector2 endPosition = new Vector2Int(5, 5);
            EntityModel enemy = (EntityModel)AddEntity(initialPosition.ToVector3Int(), false).Model;
            SimulatedBoardState simulation = new SimulatedBoardState(State.Room);
            MoveEntityStateChange moveChange = new MoveEntityStateChange(enemy, new List<Vector2>() { initialPosition, endPosition });

            moveChange.ApplyChangeToSimulation(simulation);

            PositionPropertySnapshot position = simulation.GetProperty<PositionPropertySnapshot>(enemy);
            Assert.That(position.Position.ToVector2(), Is.EqualTo(endPosition));
            Assert.That(simulation.Board.TryGetUnits(endPosition, out HashSet<AIUnitModel> units), Is.True);
            Assert.That(units.Count, Is.EqualTo(1));
            Assert.That(units.First(), Is.EqualTo(enemy));
            Assert.That(simulation.Board.TryGetUnits(initialPosition, out _), Is.False);
            Assert.That(simulation.Board.GetNodeType(initialPosition), Is.EqualTo(BoardNodeType.Walkable));
            Assert.That(simulation.Board.GetNodeType(endPosition), Is.EqualTo(BoardNodeType.Obstacle));
        }
    }
}