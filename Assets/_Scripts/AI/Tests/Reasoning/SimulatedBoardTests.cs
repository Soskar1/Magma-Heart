using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class SimulatedBoardTests
    {
        private Board m_board;
        private Entity m_entity;
        private CommandRunner m_commandRunner;

        [SetUp]
        public void SetUp()
        {
            m_entity = new Entity(false, 0);
            m_entity.CurrentHealth = 10;
            m_entity.Position = Vector2.zero;

            BoardGraph graph = new BoardGraph();
            graph.AddNode(Vector2.zero, BoardNodeType.Obstacle);
            graph.AddNode(Vector2.up, BoardNodeType.Walkable);
            graph.AddNode(new Vector2(1, 1), BoardNodeType.Walkable);

            m_board = new Board(graph);
            m_board.AddUnit(m_entity.Position, m_entity);

            m_commandRunner = new CommandRunner();
        }

        [Test]
        public void Undo_RemovesLastStateChangeFromHistoryStack()
        {
            MoveCommand movementStateChange = new MoveCommand(m_entity.Id, Vector2.zero, Vector2.up);
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange });

            m_commandRunner.Undo(m_board);

            m_board.TryGetUnit(m_entity.id, out Entity simulatedEntity);
            Assert.That(m_commandRunner.History.Count, Is.EqualTo(0));
            Assert.That(simulatedEntity.Position, Is.EqualTo(Vector2.zero));
            Assert.That(m_board.GetNodeType(Vector2.zero), Is.EqualTo(BoardNodeType.Obstacle));
            Assert.That(m_board.GetNodeType(Vector2.up), Is.EqualTo(BoardNodeType.Walkable));
        }

        [Test]
        public void Undo_TwoSimulations_RemovesLastStateChangeFromHistoryStack()
        {
            MoveCommand movementStateChange = new MoveCommand(m_entity.Id, Vector2.zero, Vector2.up);
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange });
            movementStateChange = new MoveCommand(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange });

            m_commandRunner.Undo(m_board);

            Assert.That(m_commandRunner.History.Count, Is.EqualTo(1));
            m_board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(Vector2.up));
        }

        [Test]
        public void Undo_MultipleStateChangesInOneSimulation_RemovesLastStateChangeFromHistoryStack()
        {
            MoveCommand movementStateChange1 = new MoveCommand(m_entity.Id, Vector2.zero, Vector2.up);
            MoveCommand movementStateChange2 = new MoveCommand(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange1, movementStateChange2 });

            m_commandRunner.Undo(m_board);

            Assert.That(m_commandRunner.History.Count, Is.EqualTo(0));
            m_board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(Vector2.zero));
        }

        [Test]
        public void Undo_TwoSimulationsWithMultipleStateChangesInOneSimulation_RemovesLastStateChangeFromHistoryStack()
        {
            MoveCommand movementStateChange1 = new MoveCommand(m_entity.Id, Vector2.zero, Vector2.up);
            MoveCommand movementStateChange2 = new MoveCommand(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange1, movementStateChange2 });
            movementStateChange1 = new MoveCommand(m_entity.Id, new Vector2(1, 1), Vector2.up);
            movementStateChange2 = new MoveCommand(m_entity.Id, Vector2.up, Vector2.zero);
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange1, movementStateChange2 });

            m_commandRunner.Undo(m_board);

            Assert.That(m_commandRunner.History.Count, Is.EqualTo(1));
            m_board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(new Vector2(1, 1)));
        }

        [Test]
        public void Undo_TwoSimulationsWithMultipleStateChangesInOneSimulationMultipleUndos_ReturnToInitialState()
        {
            MoveCommand movementStateChange1 = new MoveCommand(m_entity.Id, Vector2.zero, Vector2.up);
            MoveCommand movementStateChange2 = new MoveCommand(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange1, movementStateChange2 });
            movementStateChange1 = new MoveCommand(m_entity.Id, new Vector2(1, 1), Vector2.up);
            movementStateChange2 = new MoveCommand(m_entity.Id, Vector2.up, Vector2.zero);
            m_commandRunner.Apply(m_board, new List<IBoardCommand>() { movementStateChange1, movementStateChange2 });

            m_commandRunner.Undo(m_board);
            m_commandRunner.Undo(m_board);

            Assert.That(m_commandRunner.History.Count, Is.EqualTo(0));
            m_board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(Vector2.zero));
        }
    }
}
