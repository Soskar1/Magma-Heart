using System;
using System.Collections.Generic;
using MagmaHeart.AI.Boards;
using MagmaHeart.Collections;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.AI.Pathifinding
{
    public class AStar
    {
        private Func<Vector2, Vector2, float> m_heuristic;

        public static Func<Vector2, Vector2, float> ManhattanDistance = (v1, v2) => Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y);
        public static Func<Vector2, Vector2, float> EuclideanDistance = (v1, v2) => Vector2.Distance(v1, v2);

        public AStar(Func<Vector2, Vector2, float> heuristic)
        {
            heuristic.ThrowIfNull(nameof(heuristic));

            m_heuristic = heuristic;
        }

        private class ComputationalBoardNode
        {
            public BoardNode Node { get; init; }
            public ComputationalBoardNode Parent { get; set; }
            public float PathCost { get; set; }
            public float HeuristicCost { get; set; }
            public float TotalCost => PathCost + HeuristicCost;

            public ComputationalBoardNode(BoardNode node, float pathCost, float heuristicCost, ComputationalBoardNode parent = null)
            {
                Node = node;
                PathCost = pathCost;
                HeuristicCost = heuristicCost;
                Parent = parent;
            }
        }

        public List<Vector2> FindPath(BoardGraph graph, Vector2 start, Vector2 target)
        {
            if (!graph.ContainsNode(start) || !graph.ContainsNode(target))
            {
                Debug.LogWarning($"Can't find a path between {start} and {target}. One of the nodes does not exist!");
                return null;
            }

            BoardNode startNode = graph.GetNode(start);
            ComputationalBoardNode startComputationalNode = new ComputationalBoardNode(startNode, 0, m_heuristic(start, target));

            HashSet<BoardNode> visitedNodes = new HashSet<BoardNode>();
            
            Dictionary<BoardNode, ComputationalBoardNode> nodesInProcess = new Dictionary<BoardNode, ComputationalBoardNode>();
            nodesInProcess.Add(startNode, startComputationalNode);

            PriorityQueue<ComputationalBoardNode, float> nodeQueue = new PriorityQueue<ComputationalBoardNode, float>();
            nodeQueue.Enqueue(startComputationalNode, startComputationalNode.TotalCost);

            while (nodeQueue.Count > 0)
            {
                ComputationalBoardNode current = nodeQueue.Dequeue();

                if (current.Node.Position == target)
                    return ReconstructPath(current);

                visitedNodes.Add(current.Node);

                IEnumerable<BoardNode> adjacentNodes = graph.GetAdjacentNodes(current.Node.Position);
                foreach (BoardNode adjacentNode in adjacentNodes)
                {
                    if (!visitedNodes.Contains(adjacentNode) && adjacentNode.Type == BoardNodeType.Walkable)
                    {
                        float pathCost = current.PathCost + graph.GetCost(current.Node, adjacentNode);

                        if (!nodesInProcess.ContainsKey(adjacentNode))
                        {
                            ComputationalBoardNode computationalAdjacentNode =
                                new ComputationalBoardNode(adjacentNode, pathCost, m_heuristic(adjacentNode.Position, target), current);

                            nodeQueue.Enqueue(computationalAdjacentNode, computationalAdjacentNode.TotalCost);
                            nodesInProcess.Add(adjacentNode, computationalAdjacentNode);
                        }
                        else
                        {
                            ComputationalBoardNode computationalAdjacentNode = nodesInProcess[adjacentNode];
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

        private List<Vector2> ReconstructPath(ComputationalBoardNode current)
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