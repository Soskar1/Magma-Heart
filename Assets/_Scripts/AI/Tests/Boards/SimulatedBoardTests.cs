using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Boards.Tests
{
    internal class SimulatedBoardTests
    {
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

            m_board.ApplyBoardModification(0, modification);

            Assert.That(m_board.Graph.GetNode(Vector2.zero).Type, Is.EqualTo(BoardNodeType.Walkable));
        }

        [Test]
        public void UndoBoardModification_OneModification_RemovesBoardModification()
        {
            NodeTypeBoardModification modification = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.Walkable);
            m_board.ApplyBoardModification(0, modification);

            m_board.UndoBoardModification(0);

            Assert.That(m_board.Graph.GetNode(Vector2.zero).Type, Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void UndoBoardModification_TwoModificationApplied_RemovesAllBoardModifications()
        {
            NodeTypeBoardModification modification1 = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.Walkable);
            NodeTypeBoardModification modification2 = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.None);
            m_board.ApplyBoardModification(0, modification1);
            m_board.ApplyBoardModification(0, modification2);

            m_board.UndoBoardModification(0);

            Assert.That(m_board.Graph.GetNode(Vector2.zero).Type, Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void UndoBoardModification_TwoDifferentActions_RemovesOneActionsModifications()
        {
            NodeTypeBoardModification modification1 = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.Walkable);
            NodeTypeBoardModification modification2 = new NodeTypeBoardModification(Vector2.zero, BoardNodeType.None);
            m_board.ApplyBoardModification(0, modification1);
            m_board.ApplyBoardModification(1, modification2);

            m_board.UndoBoardModification(1);

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
