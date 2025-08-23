using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Navigation
{
    public class AStarGraph
    {
        private Dictionary<Vector2, AStarNode> m_nodes;
        private Dictionary<Vector2, HashSet<AStarEdge>> m_edges;
        public const float NOT_VALID_COST = -1;

        internal readonly Func<Vector2, string> m_addNodeWarningMessage = (Vector2 pos) => $"Node {pos} already exists in the map.";
        internal readonly Func<Vector2, string> m_removeNodeWarningMessage = (Vector2 pos) => $"Node at position {pos} does not exist in the map.";
        internal readonly Func<Vector2, string> m_changeNodeTypeWarningMessage = (Vector2 pos) => $"Node {pos} does not exist. Can't change type.";
        internal readonly Func<Vector2, Vector2, string> m_connectNodesNonExistingNodesWarningMessage =
            (Vector2 firstNode, Vector2 secondNode) => $"Cannot connect nodes {firstNode} and {secondNode} because one or both do not exist.";
        internal readonly Func<Vector2, Vector2, string> m_connectNodesDuplicateEdgeWarningMessage =
            (Vector2 firstNode, Vector2 secondNode) => $"Edge between {firstNode} and {secondNode} exists";
        internal readonly Func<Vector2, Vector2, string> m_removeEdgeNonExistingNodesWarningMessage =
            (Vector2 firstNode, Vector2 secondNode) => $"Cannot remove edge between {firstNode} and {secondNode} because one or both nodes do not exist.";
        internal readonly Func<Vector2, Vector2, string> m_removeEdgeNonExistingEdge =
            (Vector2 firstNode, Vector2 secondNode) => $"Edge between {firstNode} and {secondNode} does not exist.";
        internal readonly Func<Vector2, Vector2, string> m_updateCostNonExistingNodesWarningMessage =
            (Vector2 firstNode, Vector2 secondNode) => $"Cannot update cost for {firstNode} and {secondNode} edge because one or both nodes do not exist.";
        internal readonly Func<Vector2, Vector2, string> m_updateCostNonExistingEdgeWarningMessage =
            (Vector2 firstNode, Vector2 secondNode) => $"Edge between {firstNode} and {secondNode} does not exist.";

        public int NodeCount => m_nodes.Count;
        public int EdgeCount { get; private set; }

        public AStarGraph()
        {
            m_nodes = new Dictionary<Vector2, AStarNode>();
            m_edges = new Dictionary<Vector2, HashSet<AStarEdge>>();
        }

        public void AddNode(AStarNode node)
        {
            node.ThrowIfNull(nameof(node));

            if (m_nodes.ContainsKey(node.Position))
            {
                Debug.LogWarning(m_addNodeWarningMessage(node.Position));
                return;
            }

            m_nodes[node.Position] = node;
        }

        public AStarNode GetNode(Vector2 position)
        {
            if (m_nodes.TryGetValue(position, out AStarNode node))
                return node;
            else
                return null;
        }

        public void RemoveNode(Vector2 position)
        {
            if (m_nodes.ContainsKey(position))
                m_nodes.Remove(position);
            else
                Debug.LogWarning(m_removeNodeWarningMessage(position));
        }

        public void ChangeNodeType(Vector2 position, AStarNodeType newType)
        {
            AStarNode node = GetNode(position);
            if (node == null)
            {
                Debug.LogWarning(m_changeNodeTypeWarningMessage(position));
                return;
            }

            node.Type = newType;
        }

        public void ConnectNodes(Vector2 node1, Vector2 node2, float cost)
        {
            if (!ContainsNode(node1) || !ContainsNode(node2))
            {
                Debug.LogWarning(m_connectNodesNonExistingNodesWarningMessage(node1, node2));
                return;
            }

            if (ContainsEdge(node1, node2))
            {
                Debug.LogWarning(m_connectNodesDuplicateEdgeWarningMessage(node1, node2));
                return;
            }

            if (!m_edges.ContainsKey(node1))
                m_edges[node1] = new HashSet<AStarEdge>();

            if (!m_edges.ContainsKey(node2))
                m_edges[node2] = new HashSet<AStarEdge>();

            AStarEdge edge = new AStarEdge(m_nodes[node1], m_nodes[node2], cost);
            m_edges[node1].Add(edge);
            m_edges[node2].Add(edge);
            ++EdgeCount;
        }

        public void ConnectNodes(AStarNode node1, AStarNode node2, float cost) => ConnectNodes(node1.Position, node2.Position, cost);

        public void RemoveEdge(Vector2 node1, Vector2 node2)
        {
            if (!ContainsNode(node1) || !ContainsNode(node2))
            {
                Debug.LogWarning(m_removeEdgeNonExistingNodesWarningMessage(node1, node2));
                return;
            }

            if (!ContainsEdge(node1, node2))
            {
                Debug.LogWarning(m_removeEdgeNonExistingEdge(node1, node2));
                return;
            }

            AStarEdge edgeToRemove = GetEdge(node1, node2);
            m_edges[node1].Remove(edgeToRemove);
            m_edges[node2].Remove(edgeToRemove);

            if (m_edges[node1].Count == 0)
                m_edges.Remove(node1);

            if (m_edges[node2].Count == 0)
                m_edges.Remove(node2);

            --EdgeCount;
        }

        public void UpdateCost(Vector2 node1, Vector2 node2, float newCost)
        {
            if (!ContainsNode(node1) || !ContainsNode(node2))
            {
                Debug.LogWarning(m_updateCostNonExistingNodesWarningMessage(node1, node2));
                return;
            }

            if (!ContainsEdge(node1, node2))
            {
                Debug.LogWarning(m_updateCostNonExistingEdgeWarningMessage(node1, node2));
                return;
            }

            AStarEdge edgeToUpdate = GetEdge(node1, node2);
            edgeToUpdate.Cost = newCost;
        }

        public AStarEdge GetEdge(Vector2 node1, Vector2 node2)
        {
            if (!ContainsEdge(node1, node2))
                return null;

            return GetEdgeBetweenNodes(node1, node2);
        }

        public IEnumerable<AStarNode> GetAdjacentNodes(Vector2 node)
        {
            if (m_edges.ContainsKey(node))
            {
                foreach (AStarEdge edge in m_edges[node])
                {
                    if (edge.First.Position == node)
                        yield return edge.Second;
                    else
                        yield return edge.First;
                }
            }
        }

        public IEnumerable<AStarNode> GetAdjacentNodes(AStarNode node) => GetAdjacentNodes(node.Position);

        public float GetCost(Vector2 node1, Vector2 node2)
        {
            if (!ContainsEdge(node1, node2))
                return NOT_VALID_COST;

            return GetEdge(node1, node2).Cost;
        }

        public float GetCost(AStarNode node1, AStarNode node2) => GetCost(node1.Position, node2.Position);

        public bool ContainsNode(Vector2 position) => m_nodes.ContainsKey(position);

        private bool ContainsEdge(Vector2 node1, Vector2 node2)
        {
            if (!ContainsNode(node1) || !ContainsNode(node2))
                return false;

            if (!m_edges.ContainsKey(node1) || !m_edges.ContainsKey(node2))
                return false;

            return GetEdgeBetweenNodes(node1, node2) != null;
        }

        private AStarEdge GetEdgeBetweenNodes(Vector2 node1, Vector2 node2)
        {
            return m_edges[node1].Where(e =>
                (e.First.Position == node1 && e.Second.Position == node2) ||
                (e.First.Position == node2 && e.Second.Position == node1))
                .FirstOrDefault();
        }
    }
}