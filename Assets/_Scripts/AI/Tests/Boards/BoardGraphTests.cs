using NUnit.Framework;
using UnityEngine;
using System;
using UnityEngine.TestTools;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Boards.Tests
{
    internal class BoardGraphTests
    {
        [Test]
        public void AddNode_OneNode_Success()
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
        public void AddNode_PassNull_ShouldThrowException()
        {
            BoardGraph graph = new BoardGraph();

            Assert.Throws<ArgumentNullException>(() => graph.AddNode(null));
        }

        [Test]
        public void AddNode_DuplicateNode_WillNotAddItAndLogWarning()
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
        public void AddNode_MultipleDistinctNodes_AllNodesAdded()
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
        public void GetNode_PassNonExistingPositionInGraph_ReturnsNull()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node = graph.GetNode(Vector2.zero);
            Assert.IsNull(node);
        }

        [Test]
        public void RemoveNode_ExistingNode_Success()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.RemoveNode(Vector2.zero);

            Assert.That(graph.NodeCount, Is.EqualTo(0));
        }

        [Test]
        public void RemoveNode_NonExistingNode_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            graph.RemoveNode(Vector2.zero);

            LogAssert.Expect(graph.m_removeNodeWarningMessage(Vector2.zero));
        }

        [Test]
        public void ChangeNodeType_ExistingNode_Success()
        {
            BoardGraph graph = new BoardGraph();

            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.ChangeNodeType(Vector2.zero, BoardNodeType.Obstacle);
            Assert.That(node.Type, Is.EqualTo(BoardNodeType.Obstacle));
        }

        [Test]
        public void ChangeNodeType_NonExistingNode_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            graph.ChangeNodeType(Vector2.zero, BoardNodeType.Walkable);

            LogAssert.Expect(graph.m_changeNodeTypeWarningMessage(Vector2.zero));
        }

        [Test]
        public void ConnectNodes_AddsNewEdgeToTheGraph()
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
        public void ConnectNodes_MultipleValidEdges_AddsAllNewEdges()
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
        public void ConnectNodes_NonExistingNodeWithExistingNode_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);

            graph.AddNode(node);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_connectNodesNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void ConnectNodes_NonExistingNodes_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            graph.ConnectNodes(Vector2.zero, Vector2.up, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_connectNodesNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void ConnectNodes_SameNodesMultipleTime_LogsWarning()
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
        public void RemoveEdge_ExistingEdge_EdgeRemovedSuccessfully()
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
        public void RemoveEdge_OneNodeDoesNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.RemoveEdge(Vector2.zero, Vector2.up);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void RemoveEdge_NodesDoNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();

            graph.RemoveEdge(Vector2.zero, Vector2.up);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void RemoveEdge_NonExistingEdge_LogsWarning()
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
        public void UpdateCost_ExistingEdge_CostUpdated()
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
        public void UpdateCost_OneNodeDoesNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node);

            graph.UpdateCost(node.Position, Vector2.up, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingNodesWarningMessage(node.Position, Vector2.up));
        }

        [Test]
        public void UpdateCost_NodesDoNotExist_LogsWarning()
        {
            BoardGraph graph = new BoardGraph();

            graph.UpdateCost(Vector2.zero, Vector2.up, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void UpdateCost_NonExistingEdge_LogsWarning()
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
        public void GetAdjacentNodes_FromNodeWithMultipleAdjacentNodes_AllAdjacentNodesExist()
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
        public void GetAdjacentNodes_PassedIsolatedNode_IsolatedNodeWillNotGiveAnyAdjacentNodes()
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
        public void GetAdjacentNodes_OnNodeThatDoNotHaveConnectionWithIsolatedNode_IsolatedNodeIgnored()
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
        public void GetCost_ExistingEdge_ReturnsCost()
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
        public void GetCost_NonExistingEdge_ReturnsNotValidCost()
        {
            BoardGraph graph = new BoardGraph();

            float cost = graph.GetCost(Vector2.zero, Vector2.up);
            Assert.That(cost, Is.EqualTo(BoardGraph.NOT_VALID_COST));
        }

        [Test]
        public void DeepCopy_ReturnsGraphCopy()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Obstacle);
            BoardNode node3 = new BoardNode(Vector2.right, BoardNodeType.Obstacle);
            BoardNode node4 = new BoardNode(Vector2.up + Vector2.right, BoardNodeType.None);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);
            graph.ConnectNodes(node1.Position, node2.Position, 1);
            graph.ConnectNodes(node1.Position, node3.Position, 2);
            graph.ConnectNodes(node1.Position, node4.Position, 3);
            graph.ConnectNodes(node2.Position, node4.Position, 4);
            graph.ConnectNodes(node3.Position, node4.Position, 5);

            BoardGraph copy = graph.DeepCopy();

            BoardNode nodeCopy1 = copy.GetNode(Vector2.zero);
            BoardNode nodeCopy2 = copy.GetNode(Vector2.up);
            BoardNode nodeCopy3 = copy.GetNode(Vector2.right);
            BoardNode nodeCopy4 = copy.GetNode(Vector2.up + Vector2.right);
            BoardEdge edgeCopy1 = copy.GetEdge(nodeCopy1.Position, nodeCopy2.Position);
            BoardEdge edgeCopy2 = copy.GetEdge(nodeCopy1.Position, nodeCopy3.Position);
            BoardEdge edgeCopy3 = copy.GetEdge(nodeCopy1.Position, nodeCopy4.Position);
            BoardEdge edgeCopy4 = copy.GetEdge(nodeCopy2.Position, nodeCopy4.Position);
            BoardEdge edgeCopy5 = copy.GetEdge(nodeCopy3.Position, nodeCopy4.Position);
            Assert.That(nodeCopy1.Type, Is.EqualTo(BoardNodeType.Walkable));
            Assert.That(nodeCopy2.Type, Is.EqualTo(BoardNodeType.Obstacle));
            Assert.That(nodeCopy3.Type, Is.EqualTo(BoardNodeType.Obstacle));
            Assert.That(nodeCopy4.Type, Is.EqualTo(BoardNodeType.None));
            Assert.That(edgeCopy1.Cost, Is.EqualTo(1));
            Assert.That(edgeCopy2.Cost, Is.EqualTo(2));
            Assert.That(edgeCopy3.Cost, Is.EqualTo(3));
            Assert.That(edgeCopy4.Cost, Is.EqualTo(4));
            Assert.That(edgeCopy5.Cost, Is.EqualTo(5));
            Assert.That(copy.GetEdge(nodeCopy2.Position, nodeCopy3.Position), Is.Null);
            Assert.That(ReferenceEquals(nodeCopy1, node1), Is.False);
            Assert.That(ReferenceEquals(nodeCopy2, node2), Is.False);
            Assert.That(ReferenceEquals(nodeCopy3, node3), Is.False);
            Assert.That(ReferenceEquals(nodeCopy4, node4), Is.False);
        }

        [Test]
        public void GetEdges_FromNodeWithMultipleEdges_ReturnsAllEdges()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            BoardNode node2 = new BoardNode(Vector2.up, BoardNodeType.Obstacle);
            BoardNode node3 = new BoardNode(Vector2.right, BoardNodeType.Obstacle);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.ConnectNodes(node1.Position, node2.Position, 1);
            graph.ConnectNodes(node1.Position, node3.Position, 2);

            HashSet<BoardEdge> edges = graph.GetEdges(node1.Position);

            Assert.That(edges.Count, Is.EqualTo(2));
            Assert.That(edges.Any(e => (e.First == node1 && e.Second == node2) || (e.First == node2 && e.Second == node1)));
            Assert.That(edges.Any(e => (e.First == node1 && e.Second == node3) || (e.First == node3 && e.Second == node1)));
        }

        [Test]
        public void GetEdges_FromIsolatedNode_ReturnsNoEdges()
        {
            BoardGraph graph = new BoardGraph();
            BoardNode node1 = new BoardNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(node1);

            HashSet<BoardEdge> edges = graph.GetEdges(node1.Position);
            
            Assert.That(edges, Is.Null);
        }

        [Test]
        public void GetEdges_FromNonExistingNode_ReturnsNull()
        {
            BoardGraph graph = new BoardGraph();

            HashSet<BoardEdge> edges = graph.GetEdges(Vector2.zero);

            Assert.That(edges, Is.Null);
        }
    }
}

