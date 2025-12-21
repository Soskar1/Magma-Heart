using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Boards.Tests
{
    internal class BoardTests
    {
        private Board m_board;
        private AIUnitModel Unit => new AIUnitModel(false);

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            graph.AddNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(Vector2.up, BoardNodeType.Walkable);
            graph.AddNode(Vector2.right, BoardNodeType.Obstacle);
            graph.AddNode(Vector2.left, BoardNodeType.Obstacle);

            m_board = new Board(graph);
        }

        [Test]
        public void AddUnit_ExistingNode_AddsUnit()
        {
            AIUnitModel unit = Unit;

            m_board.AddUnit(Vector2.zero, unit);

            Assert.That(m_board.TryGetUnits(Vector2.zero, out HashSet<AIUnitModel> unitsOnBoardNode), Is.True);
            Assert.That(unitsOnBoardNode.First(), Is.EqualTo(unit));
        }

        [Test]
        public void AddUnit_NonExistingNode_ThrowsException()
        {
            AIUnitModel unit = Unit;

            Assert.Throws<ArgumentException>(() => m_board.AddUnit(new Vector2(1, 1), unit));
        }

        [Test]
        public void AddUnit_MultipleUnitsOnOneNode_AddsAnotherUnit()
        {
            AIUnitModel unit1 = Unit;
            AIUnitModel unit2 = Unit;
            m_board.AddUnit(Vector2.zero, unit1);

            m_board.AddUnit(Vector2.zero, unit2);

            Assert.That(m_board.TryGetUnits(Vector2.zero, out HashSet<AIUnitModel> unitsOnBoardNode), Is.True);
            Assert.That(unitsOnBoardNode.Count, Is.EqualTo(2));
        }

        [Test]
        public void RemoveUnit_ExistingUnit_RemovesUnit()
        {
            AIUnitModel unit = Unit;
            m_board.AddUnit(Vector2.zero, unit);

            bool result = m_board.RemoveUnit(Vector2.zero, unit);

            Assert.That(result, Is.True);
            Assert.That(m_board.TryGetUnits(Vector2.zero, out _), Is.False);
        }

        [Test]
        public void RemoveUnit_NonExistingUnit_DoNothing()
        {
            AIUnitModel unit = Unit;

            bool result = m_board.RemoveUnit(Vector2.zero, unit);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetUnit_ExistingUnit_ReturnsTrueAndUnit()
        {
            AIUnitModel unit = Unit;
            m_board.AddUnit(Vector2.zero, unit);

            bool result = m_board.TryGetUnits(Vector2.zero, out HashSet<AIUnitModel> unitOnBoard);

            Assert.That(result, Is.True);
            Assert.That(unitOnBoard.First(), Is.EqualTo(unit));
        }

        [Test]
        public void TryGetUnit_NonExistingUnit_ReturnsFalseAndNullUnit()
        {
            bool result = m_board.TryGetUnits(Vector2.zero, out HashSet<AIUnitModel> units);

            Assert.That(result, Is.False);
            Assert.That(units, Is.Null);
        }

        [Test]
        public void DeepCopy_ReturnsBoardCopy()
        {
            AIUnitModel unit1 = Unit;
            AIUnitModel unit2 = Unit;
            m_board.AddUnit(Vector2.zero, unit1);
            m_board.AddUnit(Vector2.up, unit2);

            Board copy = m_board.DeepCopy();

            Assert.That(ReferenceEquals(m_board, copy), Is.False);
            Assert.That(copy.TryGetUnits(Vector2.zero, out HashSet<AIUnitModel> unitCopy1), Is.True);
            Assert.That(copy.TryGetUnits(Vector2.up, out HashSet<AIUnitModel> unitCopy2), Is.True);
            Assert.That(ReferenceEquals(copy.Graph, m_board.Graph), Is.False);
        }
    }
}