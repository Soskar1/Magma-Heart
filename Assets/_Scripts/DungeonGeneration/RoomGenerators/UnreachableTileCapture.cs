using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.DungeonGeneration.RoomGeneration
{
    public class UnreachableTileCapture : IRoomGenerator
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

        public void GenerateRoom(in RoomModel roomModel)
        {
            HashSet<Vector2Int> visitedTiles = new HashSet<Vector2Int>();
            Queue<Vector2Int> tilesToVisit = new Queue<Vector2Int>();
            tilesToVisit.Enqueue(roomModel.WorldPosition);

            while (tilesToVisit.Count > 0)
            {
                Vector2Int tile = tilesToVisit.Dequeue();
                visitedTiles.Add(tile);

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    Vector2Int neighbourTile = tile + direction;

                    if (roomModel.ContainsTileAtPosition(neighbourTile) && !visitedTiles.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                        tilesToVisit.Enqueue(neighbourTile);
                }

                foreach (Vector2Int direction in m_captureDirections)
                {
                    Vector2Int tileToCapture = tile + direction;
                    Vector2Int xDirectionTile = new Vector2Int(tile.x + direction.x, tile.y);
                    Vector2Int yDirectionTile = new Vector2Int(tile.x, tile.y + direction.y);

                    if (roomModel.ContainsTileAtPosition(tileToCapture) && !roomModel.ContainsTileAtPosition(xDirectionTile) && !roomModel.ContainsTileAtPosition(yDirectionTile))
                    {
                        roomModel.AddTile(xDirectionTile, TileType.Floor);
                        roomModel.AddTile(yDirectionTile, TileType.Floor);

                        if (!tilesToVisit.Contains(tileToCapture) && !visitedTiles.Contains(tileToCapture))
                            tilesToVisit.Enqueue(tileToCapture);
                    }
                }
            }
        }
    }
}