using System.Collections.Generic;
using MagmaHeart.Navigation;
using NUnit.Framework;
using UnityEngine;

namespace MagmaHeart.Tests
{
    public class AStarTests
    {
        [Test]
        public void AStar_FindPathInOneNodeGraph_ShouldReturnPathWithStartNode()
        {
            Vector2 start = Vector2.zero;

            AStarGraph graph = new AStarGraph();
            AStarNode node = new AStarNode(start, AStarNodeType.Walkable);
            graph.AddNode(node);

            AStar aStar = new AStar(AStar.ManhattanDistance);
            List<Vector2> path = aStar.FindPath(graph, start, start);

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(1));
            Assert.That(path[0], Is.EqualTo(start));
        }

        [Test]
        public void AStar_FindPathInGraphWithOneEdge_ShouldReturnPath()
        {
            Vector2 start = Vector2.zero;
            Vector2 end = Vector2.up;

            AStarGraph graph = new AStarGraph();
            graph.AddNode(start, AStarNodeType.Walkable);
            graph.AddNode(end, AStarNodeType.Walkable);
            graph.ConnectNodes(start, end, 3);

            AStar aStar = new AStar(AStar.ManhattanDistance);
            List<Vector2> path = aStar.FindPath(graph, start, end);

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(2));
            Assert.That(path[0], Is.EqualTo(start));
            Assert.That(path[1], Is.EqualTo(end));
        }

        [Test]
        public void AStar_FindPathTargetIsObstacle_ShouldReturnNull()
        {
            Vector2 start = Vector2.zero;
            Vector2 end = Vector2.up;

            AStarGraph graph = new AStarGraph();
            graph.AddNode(start, AStarNodeType.Walkable);
            graph.AddNode(end, AStarNodeType.Obstacle);
            graph.ConnectNodes(start, end, 3);

            AStar aStar = new AStar(AStar.ManhattanDistance);
            List<Vector2> path = aStar.FindPath(graph, start, end);

            Assert.That(path, Is.Null);
        }

        [Test]
        public void AStar_FindPathWithCostDecisions_ShouldReturnPath()
        {
            (AStarGraph graph, Vector2[,] nodes) = AStarGraphCreator.Create3x3Graph();

            float newCost = 10;
            graph.UpdateCost(nodes[0, 0], nodes[1, 0], newCost);
            graph.UpdateCost(nodes[0, 1], nodes[1, 1], newCost);
            graph.UpdateCost(nodes[1, 0], nodes[1, 1], newCost);
            graph.UpdateCost(nodes[1, 1], nodes[1, 2], newCost);
            graph.UpdateCost(nodes[1, 0], nodes[2, 0], newCost);
            graph.UpdateCost(nodes[1, 1], nodes[2, 1], newCost);

            AStar aStar = new AStar(AStar.ManhattanDistance);
            List<Vector2> path = aStar.FindPath(graph, nodes[0, 0], nodes[2, 0]);

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(7));
            Assert.That(path[0], Is.EqualTo(nodes[0, 0]));
            Assert.That(path[1], Is.EqualTo(nodes[0, 1]));
            Assert.That(path[2], Is.EqualTo(nodes[0, 2]));
            Assert.That(path[3], Is.EqualTo(nodes[1, 2]));
            Assert.That(path[4], Is.EqualTo(nodes[2, 2]));
            Assert.That(path[5], Is.EqualTo(nodes[2, 1]));
            Assert.That(path[6], Is.EqualTo(nodes[2, 0]));
        }

        [Test]
        public void AStar_FindPathWithNodeTypeDecisions_ReturnsNull()
        {
            (AStarGraph graph, Vector2[,] nodes) = AStarGraphCreator.Create3x3Graph();

            graph.ChangeNodeType(nodes[1, 0], AStarNodeType.Obstacle);
            graph.ChangeNodeType(nodes[1, 1], AStarNodeType.Obstacle);
            graph.ChangeNodeType(nodes[1, 2], AStarNodeType.Obstacle);

            AStar aStar = new AStar(AStar.ManhattanDistance);
            List<Vector2> path = aStar.FindPath(graph, nodes[0, 0], nodes[2, 2]);

            Assert.That(path, Is.Null);
        }

        [Test]
        public void AStar_FindPathWithNodeTypeDecisions_ReturnsPath()
        {
            (AStarGraph graph, Vector2[,] nodes) = AStarGraphCreator.Create3x3Graph();

            graph.ChangeNodeType(nodes[1, 0], AStarNodeType.Obstacle);
            graph.ChangeNodeType(nodes[1, 2], AStarNodeType.Obstacle);

            AStar aStar = new AStar(AStar.ManhattanDistance);
            List<Vector2> path = aStar.FindPath(graph, nodes[0, 0], nodes[2, 2]);

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(5));
            Assert.That(path[0], Is.EqualTo(nodes[0, 0]));
            Assert.That(path[1], Is.EqualTo(nodes[0, 1]));
            Assert.That(path[2], Is.EqualTo(nodes[1, 1]));
            Assert.That(path[3], Is.EqualTo(nodes[2, 1]));
            Assert.That(path[4], Is.EqualTo(nodes[2, 2]));
        }
    }
}