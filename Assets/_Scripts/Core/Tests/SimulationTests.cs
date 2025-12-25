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
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal class SimulationTests
    {
        private BoardDimensions m_dimensions = new BoardDimensions(Vector2Int.zero, new Vector2Int(10, 10));

        internal PlayerVsEnemyBoard EnemyAndPlayerNearEachOther()
        {
            EntityInitializationData playerData = new EntityInitializationData(new Vector3Int(2, 3), true, 5, null);
            EntityInitializationData enemyData = new EntityInitializationData(new Vector3Int(3, 3), false, 5, null);
            return BoardBuilder.PlayerVsEnemy(m_dimensions, playerData, enemyData);
        }

        internal (Board, EntityModel) BoardWithOneEntity()
        {
            EntityInitializationData entityData = new EntityInitializationData(new Vector3Int(3, 3), true, 5, null);
            Board board = BoardBuilder.CreateEmptyBoard(m_dimensions);
            EntityModel model = BoardBuilder.AddEntity(board, entityData);
            return (board, model);
        }

        [Test]
        public void ApplyDamageStateChange_OneExecution_AppliesDamageToTargetInSimulation()
        {
            PlayerVsEnemyBoard board = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(board.Board);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(board.Enemy, board.Player, 1);

            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(board.Player);
            Assert.That(health.CurrentHealth, Is.EqualTo(4));
        }

        [Test]
        public void ApplyDamageStateChange_TwoExecutions_AppliesDamageToTargetInSimulation()
        {
            PlayerVsEnemyBoard board = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(board.Board);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(board.Enemy, board.Player, 2);

            stateChange.ApplyChangeToSimulation(simulation);
            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(board.Player);
            Assert.That(health.CurrentHealth, Is.EqualTo(1));
        }

        [Test]
        public void ApplyDamageStateChange_DamageIsGreaterThanTargetsCurrentHealth_SetsTargetIsAliveToFalse()
        {
            PlayerVsEnemyBoard board = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(board.Board);
            ApplyDamageStateChange stateChange = new ApplyDamageStateChange(board.Enemy, board.Player, 6);

            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(board.Player);
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(board.Player);
            Assert.That(health.CurrentHealth, Is.EqualTo(-1));
            Assert.That(isAlive.IsAlive, Is.False);
        }

        [Test]
        public void UpdateEnergyStateChange_OneExecution_UpdatesEnergyForTargetInSimulation()
        {
            (Board board, EntityModel entity) = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(board);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity, 3);

            energyChange.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(entity);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(3));
        }

        [Test]
        public void UpdateEnergyStateChange_TwoExecutions_LastExecutionIsAppliedToSimulation()
        {
            (Board board, EntityModel entity) = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(board);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity, 3);
            UpdateEnergyStateChange energyChange2 = new UpdateEnergyStateChange(entity, 4);

            energyChange.ApplyChangeToSimulation(simulation);
            energyChange2.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(entity);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(4));
        }

        [Test]
        public void UpdateEnergyStateChange_NewEnergyValueIsAboveMaxEnergy_SetsEnergyValueToMaxEnergy()
        {
            (Board board, EntityModel entity) = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(board);
            int energyToSet = entity.Energy.MaxEnergy + 1;
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity, energyToSet);

            energyChange.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(entity);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(entity.Energy.MaxEnergy));
        }

        [Test]
        public void UpdateEnergyStateChange_NewEnergyValueIsNegative_SetsEnergyValueToZero()
        {
            (Board board, EntityModel entity) = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(board);
            UpdateEnergyStateChange energyChange = new UpdateEnergyStateChange(entity, -1);

            energyChange.ApplyChangeToSimulation(simulation);

            EnergyPropertySnapshot energy = simulation.GetProperty<EnergyPropertySnapshot>(entity);
            Assert.That(energy.CurrentEnergy, Is.EqualTo(0));
        }

        [Test]
        public void MoveEntityStateChange_UpdatesBoardNodeTypesAndMovesEntity()
        {
            (Board board, EntityModel entity) = BoardWithOneEntity();
            SimulatedBoardState simulation = new SimulatedBoardState(board);

            Vector2Int initialPosition = entity.GetCurrentTilePosition().ToVector2Int();
            Vector2Int endPosition = new Vector2Int(5, 5);
            MoveEntityStateChange moveChange = new MoveEntityStateChange(entity, new List<Vector2>() { initialPosition, endPosition });

            moveChange.ApplyChangeToSimulation(simulation);

            PositionPropertySnapshot position = simulation.GetProperty<PositionPropertySnapshot>(entity);
            Assert.That(position.Position.ToVector2Int(), Is.EqualTo(endPosition));
            Assert.That(simulation.Board.TryGetUnits(endPosition, out HashSet<AIUnitModel> units), Is.True);
            Assert.That(units.Count, Is.EqualTo(1));
            Assert.That(units.First(), Is.EqualTo(entity));
            Assert.That(simulation.Board.TryGetUnits(initialPosition, out _), Is.False);
            Assert.That(simulation.Board.GetNodeType(initialPosition), Is.EqualTo(BoardNodeType.Walkable));
            Assert.That(simulation.Board.GetNodeType(endPosition), Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void AttackStateChange_MeleeAttack_CreatesApplyDamageStateChange()
        {
            PlayerVsEnemyBoard board = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(board.Board);
            AttackStateChange stateChange = new AttackStateChange(board.Enemy, board.Player, 2, AttackType.Melee);

            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(board.Player);
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(board.Player);
            Assert.That(health.CurrentHealth, Is.EqualTo(3));
        }

        [Test]
        public void AttackStateChange_RangedAttack_CreatesApplyDamageStateChange()
        {
            PlayerVsEnemyBoard board = EnemyAndPlayerNearEachOther();
            SimulatedBoardState simulation = new SimulatedBoardState(board.Board);
            AttackStateChange stateChange = new AttackStateChange(board.Enemy, board.Player, 2, AttackType.Ranged);

            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(board.Player);
            IsAlivePropertySnapshot isAlive = simulation.GetProperty<IsAlivePropertySnapshot>(board.Player);
            Assert.That(health.CurrentHealth, Is.EqualTo(3));
        }

        [Test]
        public void AttackStateChange_RangedAttackHitsWall_DoesNotCreateApplyDamageStateChange()
        {
            Board board = BoardBuilder.CreateEmptyBoard(m_dimensions);
            EntityInitializationData playerData = new EntityInitializationData(new Vector3Int(2, 3), true, 5, null);
            EntityInitializationData enemyData = new EntityInitializationData(new Vector3Int(4, 3), false, 5, null);
            EntityModel player = BoardBuilder.AddEntity(board, playerData);
            EntityModel enemy = BoardBuilder.AddEntity(board, enemyData);
            BoardBuilder.CreateWall(board, new Vector2(3, 3));
            SimulatedBoardState simulation = new SimulatedBoardState(board);
            AttackStateChange stateChange = new AttackStateChange(enemy, player, 2, AttackType.Ranged);

            stateChange.ApplyChangeToSimulation(simulation);

            HealthPropertySnapshot health = simulation.GetProperty<HealthPropertySnapshot>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(5));
        }
    }
}