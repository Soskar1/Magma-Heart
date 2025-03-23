using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class UnreachableTileCapture : IRoomModifier
    {
        private readonly List<Vector2Int> m_captureDirections;
        private readonly List<Vector2Int> m_directionsToVisit;

        public UnreachableTileCapture()
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };

            m_captureDirections = new List<Vector2Int>()
            {
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1),
            };
        }

        public void ModifyRoom(in RoomData roomData)
        {
            HashSet<Vector2Int> visitedTiles = new HashSet<Vector2Int>();
            Queue<Vector2Int> tilesToVisit = new Queue<Vector2Int>();
            tilesToVisit.Enqueue(roomData.WorldPosition);

            while (tilesToVisit.Count > 0)
            {
                Vector2Int tile = tilesToVisit.Dequeue();
                visitedTiles.Add(tile);

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    Vector2Int neighbourTile = tile + direction;

                    if (roomData.ContainsTile(neighbourTile) && !visitedTiles.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                        tilesToVisit.Enqueue(neighbourTile);
                }

                foreach (Vector2Int direction in m_captureDirections)
                {
                    Vector2Int tileToCapture = tile + direction;
                    Vector2Int xDirectionTile = new Vector2Int(tile.x + direction.x, tile.y);
                    Vector2Int yDirectionTile = new Vector2Int(tile.x, tile.y + direction.y);

                    if (roomData.ContainsTile(tileToCapture) && !roomData.ContainsTile(xDirectionTile) && !roomData.ContainsTile(yDirectionTile))
                    {
                        roomData.AddTile(xDirectionTile);
                        roomData.AddTile(yDirectionTile);

                        if (!tilesToVisit.Contains(tileToCapture) && !visitedTiles.Contains(tileToCapture))
                            tilesToVisit.Enqueue(tileToCapture);
                    }
                }
            }
        }
    }
}