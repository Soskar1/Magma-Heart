using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Navigation
{
    public class AStarGraph // AStarGraph
    {
        private Dictionary<Vector2, AStarNode> m_nodes;
        private Dictionary<Vector2, List<AStarEdge>> m_edges;
        public const float NOT_VALID_COST = -1;

        public AStarGraph()
        {
            m_nodes = new Dictionary<Vector2, AStarNode>();
            m_edges = new Dictionary<Vector2, List<AStarEdge>>();
        }

        public void AddNode(AStarNode node)
        {
            node.ThrowIfNull(nameof(node));

            if (m_nodes.ContainsKey(node.Position))
            {
                Debug.LogWarning($"Node {node.Position} already exists in the map.");
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
                Debug.LogWarning($"Node at position {position} does not exist in the map.");
        }

        public AStarNodeType GetNodeType(Vector2 position)
        {
            AStarNode node = GetNode(position);
            return node?.Type ?? AStarNodeType.None;
        }

        public void ChangeNodeType(Vector2 position, AStarNodeType newType)
        {
            AStarNode node = GetNode(position);
            if (node == null)
            {
                Debug.LogWarning($"Node {position} does not exist. Can't change type");
                return;
            }

            node.Type = newType;
        }

        public void ConnectNodes(Vector2 node1, Vector2 node2, float cost)
        {
            if (!HasNode(node1) || !HasNode(node2))
            {
                Debug.LogWarning($"Cannot connect nodes {node1} and {node2} because one or both do not exist.");
                return;
            }

            if (HasEdge(node1, node2))
            {
                Debug.LogWarning($"Edge between {node1} and {node2} exists");
                return;
            }

            if (!m_edges.ContainsKey(node1))
                m_edges[node1] = new List<AStarEdge>();

            if (!m_edges.ContainsKey(node2))
                m_edges[node2] = new List<AStarEdge>();

            AStarEdge edge = new AStarEdge(m_nodes[node1], m_nodes[node2], cost);
            m_edges[node1].Add(edge);
            m_edges[node2].Add(edge);
        }

        public void RemoveEdge(Vector2 node1, Vector2 node2)
        {
            if (!HasNode(node1) || !HasNode(node2))
            {
                Debug.LogWarning($"Cannot remove edge between {node1} and {node2} because one or both nodes do not exist.");
                return;
            }

            if (!HasEdge(node1, node2))
            {
                Debug.LogWarning($"Edge between {node1} and {node2} does not exist");
                return;
            }

            AStarEdge edgeToRemove = GetEdge(node1, node2);
            m_edges[node1].Remove(edgeToRemove);
            m_edges[node2].Remove(edgeToRemove);

            if (m_edges[node1].Count == 0)
                m_edges.Remove(node1);

            if (m_edges[node2].Count == 0)
                m_edges.Remove(node2);
        }

        public void UpdateCost(Vector2 node1, Vector2 node2, float newCost)
        {
            if (!HasNode(node1) || !HasNode(node2))
            {
                Debug.LogWarning($"Cannot update cost for {node1} and {node2} edge because one or both nodes do not exist.");
                return;
            }

            if (!HasEdge(node1, node2))
            {
                Debug.LogWarning($"Edge between {node1} and {node2} does not exist");
                return;
            }

            AStarEdge edgeToUpdate = GetEdge(node1, node2);
            edgeToUpdate.Cost = newCost;
        }

        public AStarEdge GetEdge(Vector2 node1, Vector2 node2)
        {
            if (!HasEdge(node1, node2))
                return null;

            return GetEdgeBetweenNodes(node1, node2);
        }

        public IEnumerable<AStarNode> GetAdjacentNodes(Vector2 node)
        {
            foreach (AStarEdge edge in m_edges[node])
            {
                if (edge.First.Position == node)
                    yield return edge.Second;
                else
                    yield return edge.First;
            }
        }

        public float GetCost(Vector2 node1, Vector2 node2)
        {
            if (!HasEdge(node1, node2))
                return NOT_VALID_COST;

            return GetEdge(node1, node2).Cost;
        }

        public float GetCost(AStarNode node1, AStarNode node2) => GetCost(node1.Position, node2.Position);

        public bool HasNode(Vector2 position) => m_nodes.ContainsKey(position);

        private bool HasEdge(Vector2 node1, Vector2 node2)
        {
            if (!HasNode(node1) || !HasNode(node2))
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