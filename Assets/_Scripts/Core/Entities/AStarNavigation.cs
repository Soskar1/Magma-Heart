using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class AStarNavigation
    {
        private class AStarNode
        {
            public AStarNode Parent { get; set; }
            public Vector2Int Position { get; private set; }

            public float DistanceToStartNode { get; set; }
            public float DistanceToTargetNode { get; private set; }
            public float Heuristic => DistanceToStartNode + DistanceToTargetNode;

            public AStarNode(Vector2Int position, Vector2Int targetPosition)
            {
                Position = position;
                DistanceToStartNode = 0;
                DistanceToTargetNode = Mathf.Abs(position.x - targetPosition.x) + Mathf.Abs(position.y - targetPosition.y);
            }

            public override bool Equals(object obj) {
                return obj is AStarNode node && node.Position == Position;
            }

            public override int GetHashCode() => Position.GetHashCode();
        }

        private RoomData m_roomData;
        private List<Vector2Int> m_directionsToVisit;
        private Vector2 m_offset;

        public AStarNavigation(RoomData roomData)
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.right,
                Vector2Int.left
            };

            m_roomData = roomData;
            m_offset = new Vector2(0.5f, 0.5f);
        }

        public List<Vector2> ConstructPath(Vector2Int start, Vector2Int target)
        {
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            AStarNode startNode = new AStarNode(start, target);
            List<AStarNode> nodesToVisit = new List<AStarNode>() { startNode };

            while (nodesToVisit.Count > 0)
            {
                AStarNode currentNode = nodesToVisit[0];

                foreach (AStarNode node in nodesToVisit)
                    if (node.Heuristic < currentNode.Heuristic || (node.Heuristic == currentNode.Heuristic && node.DistanceToTargetNode < currentNode.DistanceToTargetNode))
                        currentNode = node;

                visited.Add(currentNode.Position);
                nodesToVisit.Remove(currentNode);

                if (currentNode.Position == target)
                {
                    List<Vector2> path = new List<Vector2>();
                    while (currentNode.Parent != null)
                    {
                        path.Add(currentNode.Position + m_offset);
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();
                    return path;
                }

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    Vector2Int neighbourTilePosition = currentNode.Position + direction;
                    DungeonTile tile = m_roomData.GetTile(neighbourTilePosition);
                    if (tile != null && tile.TileType == TileType.Floor && !visited.Contains(neighbourTilePosition))
                    {
                        AStarNode neighbourNode = new AStarNode(neighbourTilePosition, target);

                        float costToNeighbour = currentNode.DistanceToStartNode + 1;

                        if (!nodesToVisit.Contains(neighbourNode) || costToNeighbour < neighbourNode.DistanceToStartNode)
                        {
                            neighbourNode.DistanceToStartNode = costToNeighbour;
                            neighbourNode.Parent = currentNode;

                            if (!nodesToVisit.Contains(neighbourNode))
                                nodesToVisit.Add(neighbourNode);
                        }
                    }
                }
            }

            return null;
        }
    }
}
