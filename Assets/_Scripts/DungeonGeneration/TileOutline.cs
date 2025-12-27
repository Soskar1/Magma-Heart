using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.DungeonGeneration
{
    public static class TileOutline
    {
        public static HashSet<DungeonTile> GetOutline(HashSet<DungeonTile> tiles)
        {
            HashSet<Vector2Int> tilePositions = tiles.Select(tile => tile.Position).ToHashSet();
            HashSet<DungeonTile> outlineTiles = new HashSet<DungeonTile>();

            foreach (DungeonTile tile in tiles)
            {
                Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int adjacentTile = tile.Position + direction;
                    if (!tilePositions.Contains(adjacentTile))
                    {
                        outlineTiles.Add(tile);
                        break;
                    }
                }
            }

            return outlineTiles;
        }
    }
}