using NUnit.Framework;
using UnityEngine;
using System;
using UnityEngine.TestTools;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Pathifinding.Tests
{
    public class AStarGraphTests
    {
        [Test]
        public void AStarGraph_AddOneNode_Success()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            graph.AddNode(node);
            Assert.That(graph.NodeCount, Is.EqualTo(1));

            AStarNode result = graph.GetNode(Vector2.zero);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(node));
        }

        [Test]
        public void AStarGraph_AddNodePassNull_ShouldThrowException()
        {
            AStarGraph graph = new AStarGraph();

            Assert.Throws<ArgumentNullException>(() => graph.AddNode(null));
        }

        [Test]
        public void AStarGraph_AddDuplicateNode_WillNotAddItAndLogWarning()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.zero, AStarNodeType.Obstacle);
            graph.AddNode(node1);
            graph.AddNode(node2);

            Assert.That(graph.NodeCount, Is.EqualTo(1));
            AStarNode result = graph.GetNode(Vector2.zero);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(node1));

            LogAssert.Expect(graph.m_addNodeWarningMessage(Vector2.zero));
        }

        [Test]
        public void AStarGraph_AddMultipleDistinctNodes_AllNodesAdded()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Obstacle);
            AStarNode node3 = new AStarNode(Vector2.right, AStarNodeType.Obstacle);
            AStarNode node4 = new AStarNode(Vector2.left, AStarNodeType.None);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);

            Assert.That(graph.NodeCount, Is.EqualTo(4));
        }

        [Test]
        public void AStarGraph_GetNodePassNonExistingPositionInGraph_ReturnsNull()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node = graph.GetNode(Vector2.zero);
            Assert.IsNull(node);
        }

        [Test]
        public void AStarGraph_RemoveExistingNode_Success()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            graph.AddNode(node);

            graph.RemoveNode(Vector2.zero);

            Assert.That(graph.NodeCount, Is.EqualTo(0));
        }

        [Test]
        public void AStarGraph_RemoveNonExistingNode_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            graph.RemoveNode(Vector2.zero);

            LogAssert.Expect(graph.m_removeNodeWarningMessage(Vector2.zero));
        }

        [Test]
        public void AStarGraph_ChangeNodeTypeOnExistingNode_Success()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            graph.AddNode(node);

            graph.ChangeNodeType(Vector2.zero, AStarNodeType.Obstacle);
            Assert.That(node.Type, Is.EqualTo(AStarNodeType.Obstacle));
        }

        [Test]
        public void AStarGraph_ChangeNodeTypeOnNonExistingNode_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            graph.ChangeNodeType(Vector2.zero, AStarNodeType.Walkable);

            LogAssert.Expect(graph.m_changeNodeTypeWarningMessage(Vector2.zero));
        }

        [Test]
        public void AStarGraph_ConnectNodes_AddsNewEdgeToTheGraph()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Walkable);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 3);

            Assert.That(graph.EdgeCount, Is.EqualTo(1));

            AStarEdge edge = graph.GetEdge(Vector2.zero, Vector2.up);
            Assert.That(edge.First, Is.EqualTo(node1));
            Assert.That(edge.Second, Is.EqualTo(node2));
            Assert.That(edge.Cost, Is.EqualTo(3));
        }

        [Test]
        public void AStarGraph_ConnectMultipleValidEdges_AddsAllNewEdges()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Obstacle);
            AStarNode node3 = new AStarNode(Vector2.right, AStarNodeType.Obstacle);
            AStarNode node4 = new AStarNode(Vector2.left, AStarNodeType.None);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);

            graph.ConnectNodes(node1, node2, 1);
            graph.ConnectNodes(node1, node3, 1);
            graph.ConnectNodes(node1, node4, 1);
            graph.ConnectNodes(node2, node4, 1);
            graph.ConnectNodes(node2, node3, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(5));
        }

        [Test]
        public void AStarGraph_ConnectNonExistingNodeWithExistingNode_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node = new AStarNode(Vector2.zero, AStarNodeType.Walkable);

            graph.AddNode(node);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_connectNodesNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void AStarGraph_ConnectNonExistingNodes_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            graph.ConnectNodes(Vector2.zero, Vector2.up, 1);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_connectNodesNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void AStarGraph_ConnectSameNodesMultipleTime_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Walkable);

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
        public void AStarGraph_RemoveExistingEdge_EdgeRemovedSuccessfully()
        {
            AStarGraph graph = new AStarGraph();

            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Walkable);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(Vector2.zero, Vector2.up, 3);

            graph.RemoveEdge(Vector2.zero, Vector2.up);
            Assert.That(graph.EdgeCount, Is.EqualTo(0));

            AStarEdge edge = graph.GetEdge(Vector2.zero, Vector2.up);
            Assert.IsNull(edge);
        }

        [Test]
        public void AStarGraph_RemoveEdgeOneNodeDoesNotExist_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            graph.AddNode(node);

            graph.RemoveEdge(Vector2.zero, Vector2.up);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void AStarGraph_RemoveEdgeNodesDoNotExist_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();

            graph.RemoveEdge(Vector2.zero, Vector2.up);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void AStarGraph_RemoveNonExistingEdge_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Walkable);
            graph.AddNode(node1);
            graph.AddNode(node2);

            graph.RemoveEdge(node1.Position, node2.Position);

            Assert.That(graph.EdgeCount, Is.EqualTo(0));
            LogAssert.Expect(graph.m_removeEdgeNonExistingEdge(node1.Position, node2.Position));
        }

        [Test]
        public void AStarGraph_UpdateCostOnExistingEdge_CostUpdated()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Walkable);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(node1.Position, node2.Position, 3);

            graph.UpdateCost(node1.Position, node2.Position, 54);

            AStarEdge edge = graph.GetEdge(node1.Position, node2.Position);

            Assert.That(edge.Cost, Is.EqualTo(54));
        }

        [Test]
        public void AStarGraph_UpdateCostOneNodeDoesNotExist_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            graph.AddNode(node);

            graph.UpdateCost(node.Position, Vector2.up, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingNodesWarningMessage(node.Position, Vector2.up));
        }

        [Test]
        public void AStarGraph_UpdateCostNodesDoNotExist_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();

            graph.UpdateCost(Vector2.zero, Vector2.up, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingNodesWarningMessage(Vector2.zero, Vector2.up));
        }

        [Test]
        public void AStarGraph_UpdateCostOnNonExistingEdge_LogsWarning()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Walkable);
            graph.AddNode(node1);
            graph.AddNode(node2);

            graph.UpdateCost(node1.Position, node2.Position, 43);

            LogAssert.Expect(graph.m_updateCostNonExistingEdgeWarningMessage(node1.Position, node2.Position));
        }

        [Test]
        public void AStarGraph_GetAdjacentNodesFromNodeWithMultipleAdjacentNodes_AllAdjacentNodesExist()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Obstacle);
            AStarNode node3 = new AStarNode(Vector2.right, AStarNodeType.Obstacle);
            AStarNode node4 = new AStarNode(Vector2.left, AStarNodeType.None);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);

            graph.ConnectNodes(node1, node2, 1);
            graph.ConnectNodes(node1, node3, 1);
            graph.ConnectNodes(node1, node4, 1);
            graph.ConnectNodes(node2, node4, 1);
            graph.ConnectNodes(node2, node3, 1);

            IEnumerable<AStarNode> adjacentNodes = graph.GetAdjacentNodes(node1);
            foreach (AStarNode node in adjacentNodes)
                Assert.That(node == node2 || node == node3 || node == node4);

            adjacentNodes = graph.GetAdjacentNodes(node2);
            foreach (AStarNode node in adjacentNodes)
                Assert.That(node == node1 || node == node3 || node == node4);

            adjacentNodes = graph.GetAdjacentNodes(node3);
            foreach (AStarNode node in adjacentNodes)
                Assert.That(node == node1 || node == node2 || node == node4);

            adjacentNodes = graph.GetAdjacentNodes(node4);
            foreach (AStarNode node in adjacentNodes)
                Assert.That(node == node1 || node == node2 || node == node3);
        }

        [Test]
        public void AStarGraph_GetAdjacentNodesPassedIsolatedNode_IsolatedNodeWillNotGiveAnyAdjacentNodes()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Obstacle);
            AStarNode node3 = new AStarNode(Vector2.right, AStarNodeType.Obstacle);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);

            graph.ConnectNodes(node2, node3, 34);

            IEnumerable<AStarNode> adjacentNodes = graph.GetAdjacentNodes(node1);
            Assert.That(adjacentNodes.Count(), Is.EqualTo(0));
        }

        [Test]
        public void AStarGraph_GetAdjacentNodesOnNodeThatDoNotHaveConnectionWithIsolatedNode_IsolatedNodeIgnored()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Obstacle);
            AStarNode node3 = new AStarNode(Vector2.right, AStarNodeType.Obstacle);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);

            graph.ConnectNodes(node2, node3, 34);

            List<AStarNode> adjacentNodeList = graph.GetAdjacentNodes(node2).ToList();
            Assert.That(adjacentNodeList.Count, Is.EqualTo(1));
            Assert.That(adjacentNodeList[0], Is.EqualTo(node3));
        }

        [Test]
        public void AStarGraph_GetCostOnExistingEdge_ReturnsCost()
        {
            AStarGraph graph = new AStarGraph();
            AStarNode node1 = new AStarNode(Vector2.zero, AStarNodeType.Walkable);
            AStarNode node2 = new AStarNode(Vector2.up, AStarNodeType.Walkable);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.ConnectNodes(node1, node2, 4);

            float cost = graph.GetCost(node1, node2);
            Assert.That(cost, Is.EqualTo(4));
        }

        [Test]
        public void AStarGraph_GetCostOnNonExistingEdge_ReturnsNotValidCost()
        {
            AStarGraph graph = new AStarGraph();

            float cost = graph.GetCost(Vector2.zero, Vector2.up);
            Assert.That(cost, Is.EqualTo(AStarGraph.NOT_VALID_COST));
        }
    }
}

