using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Bresenham.Tests
{
    public class BresenhamLineTests
    {
        [Test]
        [TestCaseSource(nameof(DrawLineCases))]
        public void DrawLine_ReturnsPoints(Vector2Int start, Vector2Int end, List<Vector2Int> expected)
        {
            var points = BresenhamLine.DrawLine(start, end);

            Assert.That(points, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> DrawLineCases()
        {
            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(3, 0),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(2, 0),
                    new Vector2Int(3, 0)
                }
            ).SetName("Horizontal line (0,0) -> (3,0)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(-3, 0),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(-1, 0),
                    new Vector2Int(-2, 0),
                    new Vector2Int(-3, 0)
                }
            ).SetName("Horizontal line (0,0) -> (-3,0)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(0, 3),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, 2),
                    new Vector2Int(0, 3)
                }
            ).SetName("Vertical line (0,0) -> (0,3)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(0, -3),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(0, -2),
                    new Vector2Int(0, -3)
                }
            ).SetName("Vertical line (0,0) -> (0,-3)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(3, 3),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 1),
                    new Vector2Int(2, 2),
                    new Vector2Int(3, 3)
                }
            ).SetName("Diagonal line (0,0) -> (3,3)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(-3, -3),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-2, -2),
                    new Vector2Int(-3, -3)
                }
            ).SetName("Diagonal line (0,0) -> (-3,-3)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(3, -3),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, -1),
                    new Vector2Int(2, -2),
                    new Vector2Int(3, -3)
                }
            ).SetName("Diagonal line (0,0) -> (3,-3)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(-3, 3),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(-1, 1),
                    new Vector2Int(-2, 2),
                    new Vector2Int(-3, 3)
                }
            ).SetName("Diagonal line (0,0) -> (-3,3)");

            yield return new TestCaseData(
                Vector2Int.zero,
                new Vector2Int(3, -2),
                new List<Vector2Int>
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, -1),
                    new Vector2Int(2, -1),
                    new Vector2Int(3, -2)
                }
            ).SetName("Diagonal line (0,0) -> (3,-2)");

            yield return new TestCaseData(
                Vector2Int.zero,
                Vector2Int.zero,
                new List<Vector2Int>()
            ).SetName("Empty line (0,0) -> (0,0)");
        }
    }
}