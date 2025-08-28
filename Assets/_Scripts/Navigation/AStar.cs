using System;
using System.Collections.Generic;
using MagmaHeart.Collections;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Navigation
{
    public class AStar
    {
        private AStarGraph m_graph;
        private Func<Vector2, Vector2, float> m_heuristic;

        public static Func<Vector2, Vector2, float> ManhattanDistance = (v1, v2) => Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y);
        public static Func<Vector2, Vector2, float> EuclideanDistance = (v1, v2) => Vector2.Distance(v1, v2);

        public AStar(AStarGraph graph, Func<Vector2, Vector2, float> heuristic)
        {
            graph.ThrowIfNull(nameof(graph));
            heuristic.ThrowIfNull(nameof(heuristic));

            m_graph = graph;
            m_heuristic = heuristic;
        }

        private class ComputationalAStarNode
        {
            public AStarNode Node { get; init; }
            public ComputationalAStarNode Parent { get; set; }
            public float PathCost { get; set; }
            public float HeuristicCost { get; set; }
            public float TotalCost => PathCost + HeuristicCost;

            public ComputationalAStarNode(AStarNode node, float pathCost, float heuristicCost, ComputationalAStarNode parent = null)
            {
                Node = node;
                PathCost = pathCost;
                HeuristicCost = heuristicCost;
                Parent = parent;
            }
        }

        public List<Vector2> FindPath(Vector2 start, Vector2 target)
        {
            if (!m_graph.ContainsNode(start) || !m_graph.ContainsNode(target))
            {
                Debug.LogWarning($"Can't find a path between {start} and {target}. One of the nodes does not exist!");
                return null;
            }

            AStarNode startNode = m_graph.GetNode(start);
            ComputationalAStarNode startComputationalNode = new ComputationalAStarNode(startNode, 0, m_heuristic(start, target));

            HashSet<AStarNode> visitedNodes = new HashSet<AStarNode>();
            
            Dictionary<AStarNode, ComputationalAStarNode> nodesInProcess = new Dictionary<AStarNode, ComputationalAStarNode>();
            nodesInProcess.Add(startNode, startComputationalNode);

            PriorityQueue<ComputationalAStarNode, float> nodeQueue = new PriorityQueue<ComputationalAStarNode, float>();
            nodeQueue.Enqueue(startComputationalNode, startComputationalNode.TotalCost);

            while (nodeQueue.Count > 0)
            {
                ComputationalAStarNode current = nodeQueue.Dequeue();
                Debug.Log($"Current node: {current.Node.Position}\nTotal Cost: {current.TotalCost}");

                if (current.Node.Position == target)
                {
                    Debug.Log("Found path!");
                    return ReconstructPath(current);
                }

                visitedNodes.Add(current.Node);

                IEnumerable<AStarNode> adjacentNodes = m_graph.GetAdjacentNodes(current.Node);
                foreach (AStarNode adjacentNode in adjacentNodes)
                {
                    Debug.Log($"Analysing adjacent node: {adjacentNode.Position}");

                    if (!visitedNodes.Contains(adjacentNode) && adjacentNode.Type == AStarNodeType.Walkable)
                    {
                        float pathCost = current.PathCost + m_graph.GetCost(current.Node, adjacentNode);

                        if (!nodesInProcess.ContainsKey(adjacentNode))
                        {
                            ComputationalAStarNode computationalAdjacentNode =
                                new ComputationalAStarNode(adjacentNode, pathCost, m_heuristic(adjacentNode.Position, target), current);

                            nodeQueue.Enqueue(computationalAdjacentNode, computationalAdjacentNode.TotalCost);
                            nodesInProcess.Add(adjacentNode, computationalAdjacentNode);
                        }
                        else
                        {
                            ComputationalAStarNode computationalAdjacentNode = nodesInProcess[adjacentNode];
                            computationalAdjacentNode.Parent = current;
                            computationalAdjacentNode.PathCost = pathCost;
                            computationalAdjacentNode.HeuristicCost = m_heuristic(adjacentNode.Position, target);

                            nodeQueue.UpdatePriority(computationalAdjacentNode, computationalAdjacentNode.TotalCost);
                        }
                    }
                }
            }

            Debug.LogWarning($"Path between {start} and {target} does not exist!");
            return null;
        }

        private List<Vector2> ReconstructPath(ComputationalAStarNode current)
        {
            List<Vector2> path = new List<Vector2>();
            while (current != null)
            {
                path.Add(current.Node.Position);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}