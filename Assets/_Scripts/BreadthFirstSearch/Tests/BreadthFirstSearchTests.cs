using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.BreadthFirstSearch.Tests
{
    public class BreadthFirstSearchTests
    {
        private HashSet<Vector2> m_fullyConnectedBoard;
        private HashSet<Vector2> m_boardWithIsolatedPoint;
        private Predicate<Vector2> m_xyIsLessThan2;
        private const int BOARD_SIZE = 4;
        private Vector2 m_isolatedPoint = new Vector2(BOARD_SIZE + 2, BOARD_SIZE + 2);

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_fullyConnectedBoard = new HashSet<Vector2>();
            m_boardWithIsolatedPoint = new HashSet<Vector2>();
            m_xyIsLessThan2 = v => v.x < 2 && v.y < 2;

            // 3x3 board
            for (int x = 0; x < BOARD_SIZE; ++x)
            {
                for (int y = 0; y < BOARD_SIZE; ++y)
                {
                    m_fullyConnectedBoard.Add(new Vector2(x, y));
                    m_boardWithIsolatedPoint.Add(new Vector2(x, y));
                }
            }

            m_boardWithIsolatedPoint.Add(m_isolatedPoint);
        }

        private IEnumerable<Vector2> GetNeighboursOnFullyConnectedBoard(Vector2 source) => GetNeighbours(m_fullyConnectedBoard, source);
        private IEnumerable<Vector2> GetNeighboursOnBoardWithIsolatedPoint(Vector2 source) => GetNeighbours(m_boardWithIsolatedPoint, source);

        private IEnumerable<Vector2> GetNeighbours(HashSet<Vector2> board, Vector2 source)
        {
            Vector2 up = source + Vector2.up;
            Vector2 down = source + Vector2.down;
            Vector2 right = source + Vector2.right;
            Vector2 left = source + Vector2.left;

            if (board.Contains(up))
                yield return up;

            if (board.Contains(right))
                yield return right;

            if (board.Contains(down))
                yield return down;

            if (board.Contains(left))
                yield return left;
        }

        [Test]
        public void BreadthFirstSearch_PerformOnFullyConnectedBoard_ReturnsAllBoard()
        {
            BreadthFirstSearch<Vector2> bfs = new BreadthFirstSearch<Vector2>(GetNeighboursOnFullyConnectedBoard);

            IEnumerable<Vector2> visited = bfs.Perform(Vector2.zero);

            bool areSame = m_fullyConnectedBoard.SetEquals(visited);
            Assert.IsTrue(areSame);
        }

        [Test]
        public void BreadthFirstSearch_PerformOnBoardWithIsolatedPoint_ReturnsBoardWihtoutIsolatedPoint()
        {
            BreadthFirstSearch<Vector2> bfs = new BreadthFirstSearch<Vector2>(GetNeighboursOnBoardWithIsolatedPoint);

            HashSet<Vector2> visited = bfs.Perform(Vector2.zero).ToHashSet();

            Assert.That(visited.Count, Is.EqualTo(m_boardWithIsolatedPoint.Count - 1));
            Assert.IsFalse(visited.Contains(m_isolatedPoint));

            bool areSame = m_fullyConnectedBoard.SetEquals(visited);
            Assert.IsTrue(areSame);

            areSame = m_boardWithIsolatedPoint.SetEquals(visited);
            Assert.IsFalse(areSame);
        }

        [Test]
        public void BreadthFirstSearch_PerformOnBoardWithIsolatedPointFromIsolatedPoint_ReturnsOnlyIsolatedPoint()
        {
            BreadthFirstSearch<Vector2> bfs = new BreadthFirstSearch<Vector2>(GetNeighboursOnBoardWithIsolatedPoint);

            IEnumerable<Vector2> visited = bfs.Perform(m_isolatedPoint);

            Assert.That(visited.Count(), Is.EqualTo(1));
            Assert.That(visited.First(), Is.EqualTo(m_isolatedPoint));
        }

        [Test]
        public void BreadthFirstSearch_PerformOnFullyConnectedBoardWithNeighbourAcceptanceCriteria_ReturnsPartOfTheBoard()
        {
            BreadthFirstSearch<Vector2> bfs = new BreadthFirstSearch<Vector2>(GetNeighboursOnFullyConnectedBoard, m_xyIsLessThan2);

            IEnumerable<Vector2> visited = bfs.Perform(Vector2.zero);

            HashSet<Vector2> expectedResult = m_fullyConnectedBoard.Where(v => m_xyIsLessThan2(v)).ToHashSet();

            bool areSame = expectedResult.SetEquals(visited);
            Assert.IsTrue(areSame);
        }

        [Test]
        public void BreadthFirstSearch_PerfomOnFullyConnectedBoardWithNeighbourAcceptanceCriteriaFromPointThatIsNotPassingCriteria_ReturnsStartPoint()
        {
            BreadthFirstSearch<Vector2> bfs = new BreadthFirstSearch<Vector2>(GetNeighboursOnFullyConnectedBoard, m_xyIsLessThan2);

            Vector2 startPoint = new Vector2(3, 3);
            IEnumerable<Vector2> visited = bfs.Perform(startPoint);

            Assert.That(visited.Count(), Is.EqualTo(1));
            Assert.That(visited.First(), Is.EqualTo(startPoint));
        }
    }
}