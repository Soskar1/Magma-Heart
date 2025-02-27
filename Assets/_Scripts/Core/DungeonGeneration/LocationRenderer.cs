using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationRenderer
    {
        private readonly Tilemap m_tilemap;
        private readonly TileBase m_floorTile;
        private readonly TileBase m_wallTile;

        public LocationRenderer(in Tilemap tilemap, in TileBase floorTile, in TileBase wallTile)
        {
            m_tilemap = tilemap;
            m_floorTile = floorTile;
            m_wallTile = wallTile;
        }

        public void DrawTiles(in HashSet<Vector2Int> floorPositions)
        {
            foreach (Vector2Int floorPosition in floorPositions)
            {
                Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)floorPosition);

                if (!floorPositions.Contains(floorPosition + Vector2Int.up) || !floorPositions.Contains(floorPosition + Vector2Int.right) ||
                    !floorPositions.Contains(floorPosition + Vector2Int.down) || !floorPositions.Contains(floorPosition + Vector2Int.left) ||
                    !floorPositions.Contains(floorPosition + new Vector2Int(1, 1)) || !floorPositions.Contains(floorPosition + new Vector2Int(1, -1)) ||
                    !floorPositions.Contains(floorPosition + new Vector2Int(-1, -1)) || !floorPositions.Contains(floorPosition + new Vector2Int(-1, 1))) {
                    m_tilemap.SetTile(tilePosition, m_wallTile);
                } else
                {
                    m_tilemap.SetTile(tilePosition, m_floorTile);
                }
            }
        }

        public void Clear()
        {
            m_tilemap.ClearAllTiles();
        }
    }
}