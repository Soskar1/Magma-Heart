using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class AStarNavigation
    {
        private class AStarNode
        {
            public AStarNode parent;
            public Vector2Int position;

            public float distanceToStartNode;
            public float distanceToTargetNode;
            public float Heuristic => distanceToStartNode + distanceToTargetNode;

            public AStarNode(Vector2Int position, Vector2Int targetPosition)
            {
                this.position = position;
                distanceToStartNode = 0;
                distanceToTargetNode = Vector2Int.Distance(position, targetPosition);
            }
        }

        private RoomData m_roomData;
        private List<Vector2Int> m_directionsToVisit;

        public AStarNavigation()
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.right,
                Vector2Int.left
            };
        }

        public List<Vector2Int> ConstructPath(Vector2Int start, Vector2Int target)
        {
            List<Vector2Int> visited = new List<Vector2Int>();

            AStarNode startNode = new AStarNode(start, target);
            List<AStarNode> nodesToVisit = new List<AStarNode>() { startNode };

            while (nodesToVisit.Count > 0)
            {
                AStarNode currentNode = nodesToVisit[0];

                foreach (AStarNode node in nodesToVisit)
                    if (node.Heuristic < currentNode.Heuristic || (node.Heuristic == currentNode.Heuristic && node.distanceToTargetNode < currentNode.distanceToTargetNode))
                        currentNode = node;

                visited.Add(currentNode.position);
                nodesToVisit.Remove(currentNode);

                if (currentNode.position == target)
                {
                    List<Vector2Int> path = new List<Vector2Int>() { target };
                    while (currentNode != startNode)
                    {
                        path.Add(currentNode.position);
                        currentNode = currentNode.parent;
                    }

                    return path;
                }

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    Vector2Int neighbourTilePosition = currentNode.position + direction;
                    DungeonTile tile = m_roomData.GetTile(neighbourTilePosition);
                    if (tile != null && tile.TileType == TileType.Floor && !visited.Contains(neighbourTilePosition))
                    {
                        AStarNode neighbourNode = new AStarNode(neighbourTilePosition, target);
                        nodesToVisit.Add(neighbourNode);

                        float costToNeighbour = currentNode.distanceToStartNode + 1;

                        if (!nodesToVisit.Contains(neighbourNode) || costToNeighbour < neighbourNode.distanceToStartNode)
                        {
                            neighbourNode.distanceToStartNode = costToNeighbour;
                            neighbourNode.parent = currentNode;

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
