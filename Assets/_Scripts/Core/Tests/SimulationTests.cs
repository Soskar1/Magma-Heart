using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities;
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
            AIScenario scenario = AIScenarioBuilder.Create(Board)
                .AddEntity().IsPlayer(true).At(playerPosition.x, playerPosition.y)
                .AddEntity().IsPlayer(false).At(enemyPosition.x, enemyPosition.y)
                .Build();

            Board.TryGetUnit(playerPosition, out EntityModel player);
            Board.TryGetUnit(enemyPosition, out EntityModel enemy);

            return (player, enemy);
        }

        internal EntityModel BoardWithOneEntity()
        {
            Vector2Int entityPosition = new Vector2Int(3, 3);
            AIScenario scenario = AIScenarioBuilder.Create(Board)
                .AddEntity().At(entityPosition.x, entityPosition.y)
                .Build();

            Board.TryGetUnit(entityPosition, out EntityModel entity);
            return entity;
        }

        [Test]
        public void ApplyDamageCommand_OneExecution_AppliesDamageToTargetInSimulation()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther();
            ApplyDamageCommand command = new ApplyDamageCommand(player.Id, 1);

            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Board.TryGetUnit(player.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(4));
        }

        [Test]
        public void ApplyDamageCommand_DamageIsGreaterThanTargetsCurrentHealth_DisablesTarget()
        {
            (EntityModel player, EntityModel enemy) = EnemyAndPlayerNearEachOther();
            ApplyDamageCommand command = new ApplyDamageCommand(player.Id, 6);

            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Board.TryGetUnit(player.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(0));
            Assert.That(simulatedPlayer.IsDisabled, Is.True);
        }

        [Test]
        public void ApplyDamageCommand_Undo_RevertsDamageToTargetInSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            ApplyDamageCommand command = new ApplyDamageCommand(entity.Id, 2);
            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Runner.Undo(Board);

            Board.TryGetUnit(entity.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(5));
        }

        [Test]
        public void ApplyDamageCommand_UndoAndLowHealth_RevertsDamageToTargetInSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            entity.Health.CurrentHealth = 1;
            ApplyDamageCommand command = new ApplyDamageCommand(entity.Id, 3);
            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Runner.Undo(Board);

            Board.TryGetUnit(entity.Id, out EntityModel simulatedPlayer);
            Assert.That(simulatedPlayer.Health.CurrentHealth, Is.EqualTo(1));
        }

        [Test]
        public void UpdateEnergyCommand_OneExecution_UpdatesEnergyForTargetInSimulation()
        {
            EntityModel entity = BoardWithOneEntity();
            UpdateEnergyCommand command = new UpdateEnergyCommand(entity.Id, 3);

            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Board.TryGetUnit(entity.Id, out EntityModel simulatedEntity);
            Assert.That(simulatedEntity.Energy.CurrentEnergy, Is.EqualTo(3));
        }

        [Test]
        public void UpdateEnergyCommand_NewEnergyValueIsAboveMaxEnergy_SetsEnergyValueToMaxEnergy()
        {
            EntityModel entity = BoardWithOneEntity();
            int energyToSet = entity.Energy.MaxEnergy + 1;
            UpdateEnergyCommand command = new UpdateEnergyCommand(entity.Id, energyToSet);

            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Board.TryGetUnit(entity.Id, out EntityModel simulatedEntity);
            Assert.That(simulatedEntity.Energy.CurrentEnergy, Is.EqualTo(entity.Energy.MaxEnergy));
        }

        [Test]
        public void UpdateEnergyCommand_NewEnergyValueIsNegative_SetsEnergyValueToZero()
        {
            EntityModel entity = BoardWithOneEntity();
            UpdateEnergyCommand command = new UpdateEnergyCommand(entity.Id, -1);

            Runner.Apply(Board, new List<IBoardCommand>() { command });
            
            Board.TryGetUnit(entity.Id, out EntityModel simulatedEntity);
            Assert.That(simulatedEntity.Energy.CurrentEnergy, Is.EqualTo(0));
        }

        [Test]
        public void UpdateEnergyCommand_Undo_SetsInitialValue()
        {
            EntityModel entity = BoardWithOneEntity();
            UpdateEnergyCommand command = new UpdateEnergyCommand(entity.Id, 4);
            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Runner.Undo(Board);

            Board.TryGetUnit(entity.Id, out EntityModel simulatedEntity);
            Assert.That(simulatedEntity.Energy.CurrentEnergy, Is.EqualTo(0));
        }

        [Test]
        public void MoveEntityCommand_UpdatesBoardNodeTypesAndMovesEntity()
        {
            EntityModel entity = BoardWithOneEntity();
            Vector2Int initialPosition = entity.TilePosition.ToVector2Int();
            Vector2Int endPosition = new Vector2Int(5, 5);
            MoveCommand command = new MoveCommand(entity.Id, initialPosition, endPosition);

            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Assert.That(Board.TryGetUnit(endPosition, out EntityModel simulatedEntity), Is.True);
            Assert.That(simulatedEntity.TilePosition.ToVector2Int(), Is.EqualTo(endPosition));
            Assert.That(Board.TryGetUnit(initialPosition, out _), Is.False);
            Assert.That(Board.GetNodeType(initialPosition), Is.EqualTo(BoardNodeType.Walkable));
            Assert.That(Board.GetNodeType(endPosition), Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void MoveEntityCommand_Undo_UpdatesBoardNodeTypesAndMovesEntity()
        {
            EntityModel entity = BoardWithOneEntity();
            Vector2Int initialPosition = entity.TilePosition.ToVector2Int();
            Vector2Int endPosition = new Vector2Int(5, 5);
            MoveCommand command = new MoveCommand(entity.Id, initialPosition, endPosition);
            Runner.Apply(Board, new List<IBoardCommand>() { command });

            Runner.Undo(Board);

            Assert.That(Board.TryGetUnit(endPosition, out EntityModel simulatedEntity), Is.False);
            Assert.That(Board.TryGetUnit(initialPosition, out simulatedEntity), Is.True);
            Assert.That(simulatedEntity.TilePosition.ToVector2Int(), Is.EqualTo(initialPosition));
            Assert.That(Board.GetNodeType(initialPosition), Is.EqualTo(BoardNodeType.Obstacle));
            Assert.That(Board.GetNodeType(endPosition), Is.EqualTo(BoardNodeType.Walkable));
        }
    }
}