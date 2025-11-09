using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Boards.Tests
{
    internal class SimulatedBoardTests
    {
        private class TestAction : Action
        {
            public TestAction(AIUnit actionPossessor) : base(actionPossessor) { }

            public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, AIUnit target)
            {
                throw new System.NotImplementedException();
            }

            public override void Execute()
            {
                throw new System.NotImplementedException();
            }
        }

        private SimulatedBoard m_board;

        [SetUp]
        public void SetUp()
        {
            BoardGraph boardGraph = new BoardGraph();
            boardGraph.AddNode(Vector2.zero, BoardNodeType.Obstacle);
            Dictionary<Vector2, AIUnit> units = new Dictionary<Vector2, AIUnit>()
            {
                { Vector2.zero, new AIUnit() }
            };
            m_board = new SimulatedBoard(boardGraph, units);
        }

        [Test]
        public void ApplyBoardModification_NewModificationApplied()
        {
            NodeTypeBoardModification modification = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.Walkable);

            m_board.ApplyBoardModification(new TestAction(null), modification);

            Assert.That(m_board.Graph.GetNode(Vector2.zero).Type, Is.EqualTo(BoardNodeType.Walkable));
        }

        [Test]
        public void UndoBoardModification_OneModification_RemovesBoardModification()
        {
            NodeTypeBoardModification modification = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.Walkable);
            TestAction action = new TestAction(null);
            m_board.ApplyBoardModification(action, modification);

            m_board.UndoBoardModification(action);

            Assert.That(m_board.Graph.GetNode(Vector2.zero).Type, Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void UndoBoardModification_TwoModificationApplied_RemovesLastBoardModification()
        {
            NodeTypeBoardModification modification1 = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.Walkable);
            NodeTypeBoardModification modification2 = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.None);
            TestAction action = new TestAction(null);
            m_board.ApplyBoardModification(action, modification1);
            m_board.ApplyBoardModification(action, modification2);

            m_board.UndoBoardModification(action);

            Assert.That(m_board.Graph.GetNode(Vector2.zero).Type, Is.EqualTo(BoardNodeType.Walkable));
        }

        [Test]
        public void TryGetUnitOnPosition_ValidPosition_GetsUnit()
        {
            bool result = m_board.TryGetUnitOnPosition(Vector2.zero, out AIUnit unit);

            Assert.That(result, Is.True);
            Assert.That(unit, Is.Not.Null);
        }

        [Test]
        public void TryGetUnitOnPosition_InvalidPosition_GetsNull()
        {
            bool result = m_board.TryGetUnitOnPosition(Vector2.up, out AIUnit unit);

            Assert.That(result, Is.False);
            Assert.That(unit, Is.Null);
        }
    }
}
