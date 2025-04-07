using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationWallGenerator
    {
        private readonly List<Vector2Int> m_directionsToVisit;

        public LocationWallGenerator()
        {
            m_directionsToVisit = new List<Vector2Int>()
            {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };
        }

        public HashSet<DungeonTile> GenerateWalls(HashSet<Vector2Int> tilePositions)
        {
            HashSet<DungeonTile> locationWalls = new HashSet<DungeonTile>();
            HashSet<Vector2Int> visitedTiles = new HashSet<Vector2Int>();
            
            Queue<Vector2Int> tilesToVisit = new Queue<Vector2Int>();
            Vector2Int startTile = tilePositions.First();
            tilesToVisit.Enqueue(startTile);

            while (tilesToVisit.Count > 0)
            {
                Vector2Int tile = tilesToVisit.Dequeue();
                visitedTiles.Add(tile);

                foreach (Vector2Int direction in m_directionsToVisit)
                {
                    Vector2Int neighbourTile = tile + direction;

                    if (!tilePositions.Contains(neighbourTile))
                        locationWalls.Add(new DungeonTile(neighbourTile, TileType.Wall));
                    
                    if (tilePositions.Contains(neighbourTile) && !visitedTiles.Contains(neighbourTile) && !tilesToVisit.Contains(neighbourTile))
                        tilesToVisit.Enqueue(neighbourTile);
                }
            }

            return locationWalls;
        }
    }
}