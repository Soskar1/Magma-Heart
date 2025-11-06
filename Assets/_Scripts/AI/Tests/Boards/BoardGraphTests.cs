using NUnit.Framework;
using UnityEngine;
using System;
using UnityEngine.TestTools;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Boards.Tests
{
    public class BoardGraphTests
    {
        [Test]
        public void BoardGraph_AddOneNode_Success()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);
            Assert.That(graph.NodeCount, Is.EqualTo(1));

            BoardNode result = graph.GetNode(Vector2.zero);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(node));
        }

        [Test]
        public void BoardGraph_AddNodePassNull_ShouldThrowException()
        {
            BoardGraph graph = new BoardGraph();

            Assert.Throws<ArgumentNullException>(() => graph.AddNode(null));
        }

        [Test]
        public void BoardGraph_AddDuplicateNode_WillNotAddItAndLogWarning()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.zero, BoardNodeType.Obstacle);
            graph.AddNode(node1);
            graph.AddNode(node2);

            Assert.That(graph.NodeCount, Is.EqualTo(1));
            BoardNode result = graph.GetNode(Vector2.zero);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(node1));

            LogAssert.Expect(graph.m_addNodeWarningMessage(Vector2.zero));
        }

        [Test]
        public void BoardGraph_AddMultipleDistinctNodes_AllNodesAdded()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Obstacle);
            BoardNode node3 = new BoardNode(Vector2.right, BoardNodeType.Obstacle);
            BoardNode node4 = new BoardNode(Vector2.left, BoardNodeType.None);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);

            Assert.That(graph.NodeCount, Is.EqualTo(4));
        }

        [Test]
        public void BoardGraph_GetNodePassNonExistingPositionInGraph_ReturnsNull()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node = graph.GetNode(Vector2.zero);
            Assert.IsNull(node);
        }

        [Test]
        public void BoardGraph_RemoveExistingNode_Success()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.RemoveNode(Vector2.zero);

            Assert.That(graph.NodeCount, Is.EqualTo(0));
        }

        [Test]
        public void BoardGraph_RemoveNonExistingNode_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            graph.RemoveNode(Vector2.zero);

            LogAssert.Expect(graph.m_removeNodeWarningMessage(Vector2.zero));
        }

        [Test]
        public void BoardGraph_ChangeNodeTypeOnExistingNode_Success()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.ChangeNodeType(Vector2.zero, BoardNodeType.Obstacle);
            Assert.That(node.Type, Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void BoardGraph_ChangeNodeTypeOnNonExistingNode_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            graph.ChangeNodeType(Vector2.zero, BoardNodeType.Walkable);

            LogAssert.Expect(graph.m_changeNodeTypeWarningMessage(Vector2.zero));
        }

        [Test]
        public void BoardGraph_ConnectNodes_AddsNewEdgeToTheGraph()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Walkable);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 3);

            Assert.That(graph.EdgeCount, Is.EqualTo(1));

            BoardEdge edge = graph.GetEdge(Vector2.zero, Vector2.up);
            Assert.That(edge.First, Is.EqualTo(node1));
            Assert.That(edge.Second, Is.EqualTo(node2));
            Assert.That(edge.Cost, Is.EqualTo(3));
        }

        [Test]
        public void BoardGraph_ConnectMultipleValidEdges_AddsAllNewEdges()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Obstacle);
            BoardNode node3 = new BoardNode(Vector2.right, BoardNodeType.Obstacle);
            BoardNode node4 = new BoardNode(Vector2.left, BoardNodeType.None);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);

            graph.ConnectNodes(node1.Position, node2.Position, 1);
            graph.ConnectNodes(node1.Position, node3.Position, 1);
            graph.ConnectNodes(node1.Position, node4.Position, 1);
            graph.ConnectNodes(node2.Position, node3.Position, 1);
            graph.ConnectNodes(node2.Position, node4.Position, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(5));
        }

        [Test]
        public void BoardGraph_ConnectNonExistingNodeWithExistingNode_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);

            graph.AddNode(node);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_connectNodesNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void BoardGraph_ConnectNonExistingNodes_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            graph.ConnectNodes(Vector2.zero, Vector2.up, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_connectNodesNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void BoardGraph_ConnectSameNodesMultipleTime_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Walkable);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 3);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 445);

            LogAssert.Expect(graph.m_connectNodesDuplicateEdgeWarningMessage(Vector2.zero, Vector2.up));
            Assert.That(graph.EdgeCount, Is.EqualTo(1));

            graph.ConnectNodes(Vector2.up, Vector2.zero, 445);

            Assert.That(graph.EdgeCount, Is.EqualTo(1));
            LogAssert.Expect(graph.m_connectNodesDuplicateEdgeWarningMessage(Vector2.up, Vector2.zero));
        }

        [Test]
        public void BoardGraph_RemoveExistingEdge_EdgeRemovedSuccessfully()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Walkable);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 3);

            graph.RemoveEdge(Vector2.zero, Vector2.up);
            Assert.That(graph.EdgeCount, Is.EqualTo(0));

            BoardEdge edge = graph.GetEdge(Vector2.zero, Vector2.up);
            Assert.IsNull(edge);
        }

        [Test]
        public void BoardGraph_RemoveEdgeOneNodeDoesNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.RemoveEdge(Vector2.zero, Vector2.up);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void BoardGraph_RemoveEdgeNodesDoNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();

            graph.RemoveEdge(Vector2.zero, Vector2.up);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void BoardGraph_RemoveNonExistingEdge_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Walkable);
            graph.AddNode(node1);
            graph.AddNode(node2);

            graph.RemoveEdge(node1.Position, node2.Position);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingEdge(node1.Position, node2.Position));
        }

        [Test]
        public void BoardGraph_UpdateCostOnExistingEdge_CostUpdated()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Walkable);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(node1.Position, node2.Position, 3);

            graph.UpdateCost(node1.Position, node2.Position, 54);

            BoardEdge edge = graph.GetEdge(node1.Position, node2.Position);

            Assert.That(edge.Cost, Is.EqualTo(54));
        }

        [Test]
        public void BoardGraph_UpdateCostOneNodeDoesNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.UpdateCost(node.Position, Vector2.up, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingNodesWarningMessage(node.Position, Vector2.up));
        }

        [Test]
        public void BoardGraph_UpdateCostNodesDoNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();

            graph.UpdateCost(Vector2.zero, Vector2.up, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void BoardGraph_UpdateCostOnNonExistingEdge_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Walkable);
            graph.AddNode(node1);
            graph.AddNode(node2);

            graph.UpdateCost(node1.Position, node2.Position, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingEdgeWarningMessage(node1.Position, node2.Position));
        }

        [Test]
        public void BoardGraph_GetAdjacentNodesFromNodeWithMultipleAdjacentNodes_AllAdjacentNodesExist()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Obstacle);
            BoardNode node3 = new BoardNode(Vector2.right, BoardNodeType.Obstacle);
            BoardNode node4 = new BoardNode(Vector2.left, BoardNodeType.None);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);

            graph.ConnectNodes(node1.Position, node2.Position, 1);
            graph.ConnectNodes(node1.Position, node3.Position, 1);
            graph.ConnectNodes(node1.Position, node4.Position, 1);
            graph.ConnectNodes(node2.Position, node4.Position, 1);
            graph.ConnectNodes(node2.Position, node3.Position, 1);

            IEnumerable<BoardNode> adjacentNodes = graph.GetAdjacentNodes(node1.Position);
            foreach (BoardNode node in adjacentNodes)
                Assert.That(node == node2 || node == node3 || node == node4);

            adjacentNodes = graph.GetAdjacentNodes(node2.Position);
            foreach (BoardNode node in adjacentNodes)
                Assert.That(node == node1 || node == node3 || node == node4);

            adjacentNodes = graph.GetAdjacentNodes(node3.Position);
            foreach (BoardNode node in adjacentNodes)
                Assert.That(node == node1 || node == node2 || node == node4);

            adjacentNodes = graph.GetAdjacentNodes(node4.Position);
            foreach (BoardNode node in adjacentNodes)
                Assert.That(node == node1 || node == node2 || node == node3);
        }

        [Test]
        public void BoardGraph_GetAdjacentNodesPassedIsolatedNode_IsolatedNodeWillNotGiveAnyAdjacentNodes()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Obstacle);
            BoardNode node3 = new BoardNode(Vector2.right, BoardNodeType.Obstacle);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);

            graph.ConnectNodes(node2.Position, node3.Position, 34);

            IEnumerable<BoardNode> adjacentNodes = graph.GetAdjacentNodes(node1.Position);
            Assert.That(adjacentNodes.Count(), Is.EqualTo(0));
        }

        [Test]
        public void BoardGraph_GetAdjacentNodesOnNodeThatDoNotHaveConnectionWithIsolatedNode_IsolatedNodeIgnored()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Obstacle);
            BoardNode node3 = new BoardNode(Vector2.right, BoardNodeType.Obstacle);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);

            graph.ConnectNodes(node2.Position, node3.Position, 34);

            List<BoardNode> adjacentNodeList = graph.GetAdjacentNodes(node2.Position).ToList();
            Assert.That(adjacentNodeList.Count, Is.EqualTo(1));
            Assert.That(adjacentNodeList[0], Is.EqualTo(node3));
        }

        [Test]
        public void BoardGraph_GetCostOnExistingEdge_ReturnsCost()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Walkable);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(node1.Position, node2.Position, 4);

            float cost = graph.GetCost(node1, node2);
            Assert.That(cost, Is.EqualTo(4));
        }

        [Test]
        public void BoardGraph_GetCostOnNonExistingEdge_ReturnsNotValidCost()
        {
            BoardGraph graph = new BoardGraph();

            float cost = graph.GetCost(Vector2.zero, Vector2.up);
            Assert.That(cost, Is.EqualTo(BoardGraph.NOT_VALID_COST));
        }
    }
}

