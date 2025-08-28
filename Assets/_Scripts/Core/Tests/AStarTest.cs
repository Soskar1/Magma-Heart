using MagmaHeart.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    public class AStarTest : MonoBehaviour
    {
        [Serializable]
        private class Points
        {
            public List<Transform> adjacentPoints;

            public Transform this[int i]
            {
                get => adjacentPoints[i];
                set => adjacentPoints[i] = value;
            }

            public int Count => adjacentPoints.Count;
        }

        [SerializeField] private List<Transform> m_transforms = new List<Transform>();
        [SerializeField] private List<Points> m_adjacencyMatrix = new List<Points>();
        private AStarGraph m_graph;

        [SerializeField] private Transform m_start;
        [SerializeField] private Transform m_target;

        [SerializeField][Range(0.1f, 5.0f)] private float m_nodeSize = 0.5f;

        private List<Vector2> m_currentPath = new List<Vector2>();

        public void BuildAStarGraph()
        {
            m_graph = new AStarGraph();

            foreach (Transform t in m_transforms)
            {
                AStarNode node = new AStarNode(t.position, AStarNodeType.Walkable);
                m_graph.AddNode(node);
            }

            for (int i = 0; i < m_adjacencyMatrix.Count; ++i)
            {
                Vector2 pos1 = m_adjacencyMatrix[i][0].position;
                for (int j = 1; j < m_adjacencyMatrix[i].Count; ++j)
                {
                    Vector2 pos2 = m_adjacencyMatrix[i][j].position;
                    float cost = Vector2.Distance(pos1, pos2);
                    m_graph.ConnectNodes(pos1, pos2, cost);
                }
            }

            Debug.Log("Built A* graph");
        }

        public void PerformAStar()
        {
            AStar aStar = new AStar(AStar.EuclideanDistance);

            m_currentPath = aStar.FindPath(m_graph, m_start.position, m_target.position);

            StringBuilder sb = new StringBuilder("Path: ");
            for (int i = 0; i < m_currentPath.Count; ++i)
            {
                sb.Append(m_currentPath[i]);
                if (i + 1 != m_currentPath.Count)
                    sb.Append(" -> ");
            }

            Debug.Log(sb.ToString());
        }

        private void OnDrawGizmos()
        {
            // Draw nodes
            Gizmos.color = Color.blue;
            foreach (Transform t in m_transforms)
            {
                Vector2 center = t.position;
                Vector2 bottomLeft = new Vector2(center.x - m_nodeSize, center.y - m_nodeSize);
                Vector2 topLeft = new Vector2(center.x - m_nodeSize, center.y + m_nodeSize);
                Vector2 topRight = new Vector2(center.x + m_nodeSize, center.y + m_nodeSize);
                Vector2 bottomRight = new Vector2(center.x + m_nodeSize, center.y - m_nodeSize);

                Gizmos.DrawLine(bottomLeft, topLeft);
                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
            }

            // Draw possible paths
            Gizmos.color = Color.white;
            for (int i = 0; i < m_adjacencyMatrix.Count; ++i)
            {
                Vector2 pos1 = m_adjacencyMatrix[i][0].position;
                for (int j = 1; j < m_adjacencyMatrix[i].Count; ++j)
                {
                    Vector2 pos2 = m_adjacencyMatrix[i][j].position;
                    Gizmos.DrawLine(pos1, pos2);
                }
            }

            // Draw found path
            if (m_currentPath == null)
                return;

            Gizmos.color = Color.green;
            for (int i = 0; i < m_currentPath.Count - 1; ++i)
                Gizmos.DrawLine(m_currentPath[i], m_currentPath[i + 1]);
        }
    }
}