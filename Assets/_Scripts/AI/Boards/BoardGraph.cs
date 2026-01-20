using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MagmaHeart.Extensions;

namespace MagmaHeart.AI.Boards
{
    public class BoardGraph
    {
        private Dictionary<Vector2, BoardNode> m_nodes;
        private Dictionary<Vector2, HashSet<BoardEdge>> m_edges;
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

        public BoardGraph()
        {
            m_nodes = new Dictionary<Vector2, BoardNode>();
            m_edges = new Dictionary<Vector2, HashSet<BoardEdge>>();
        }

        public void AddNode(BoardNode node)
        {
            node.ThrowIfNull(nameof(node));

            if (m_nodes.ContainsKey(node.Position))
            {
                Debug.LogWarning(m_addNodeWarningMessage(node.Position));
                return;
            }

            m_nodes[node.Position] = node;
        }

        public void AddNode(Vector2 position, BoardNodeType nodeType)
        {
            BoardNode node = new BoardNode(position, nodeType);
            AddNode(node);
        }

        internal BoardNode GetNode(Vector2 position)
        {
            if (m_nodes.TryGetValue(position, out BoardNode node))
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

        public void ChangeNodeType(Vector2 position, BoardNodeType newType)
        {
            BoardNode node = GetNode(position);
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
                m_edges[node1] = new HashSet<BoardEdge>();

            if (!m_edges.ContainsKey(node2))
                m_edges[node2] = new HashSet<BoardEdge>();

            BoardEdge edge = new BoardEdge(m_nodes[node1], m_nodes[node2], cost);
            m_edges[node1].Add(edge);
            m_edges[node2].Add(edge);
            ++EdgeCount;
        }

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

            BoardEdge edgeToRemove = GetEdge(node1, node2);
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

            BoardEdge edgeToUpdate = GetEdge(node1, node2);
            edgeToUpdate.Cost = newCost;
        }

        internal BoardEdge GetEdge(Vector2 node1, Vector2 node2)
        {
            if (!ContainsEdge(node1, node2))
                return null;

            return GetEdgeBetweenNodes(node1, node2);
        }

        internal HashSet<BoardEdge> GetEdges(Vector2 node)
        {
            if (m_edges.ContainsKey(node))
                return m_edges[node];
            else
                return null;
        }

        internal IEnumerable<BoardNode> GetAdjacentNodes(Vector2 node)
        {
            if (m_edges.ContainsKey(node))
            {
                foreach (BoardEdge edge in m_edges[node])
                {
                    if (edge.First.Position == node)
                        yield return edge.Second;
                    else
                        yield return edge.First;
                }
            }
        }

        public float GetCost(Vector2 node1, Vector2 node2)
        {
            if (!ContainsEdge(node1, node2))
                return NOT_VALID_COST;

            return GetEdge(node1, node2).Cost;
        }

        internal float GetCost(BoardNode node1, BoardNode node2) => GetCost(node1.Position, node2.Position);

        public bool ContainsNode(Vector2 position) => m_nodes.ContainsKey(position);

        private bool ContainsEdge(Vector2 node1, Vector2 node2)
        {
            if (!ContainsNode(node1) || !ContainsNode(node2))
                return false;

            if (!m_edges.ContainsKey(node1) || !m_edges.ContainsKey(node2))
                return false;

            return GetEdgeBetweenNodes(node1, node2) != null;
        }

        private BoardEdge GetEdgeBetweenNodes(Vector2 node1, Vector2 node2)
        {
            return m_edges[node1].Where(e =>
                (e.First.Position == node1 && e.Second.Position == node2) ||
                (e.First.Position == node2 && e.Second.Position == node1))
                .FirstOrDefault();
        }

        public BoardGraph DeepCopy()
        {
            BoardGraph result = new BoardGraph();

            foreach (BoardNode node in m_nodes.Values)
            {
                BoardNode copy = new BoardNode(node.Position, node.Type);
                result.AddNode(copy);
            }

            foreach (Vector2 node in m_edges.Keys)
            {
                HashSet<BoardEdge> edges = GetEdges(node);

                foreach (BoardEdge edge in edges)
                {
                    Vector2 firstPos = edge.First.Position;
                    Vector2 secondPos = edge.Second.Position;

                    if (!result.ContainsEdge(firstPos, secondPos))
                        result.ConnectNodes(firstPos, secondPos, edge.Cost);
                }
            }

            return result;
        }
    }
}