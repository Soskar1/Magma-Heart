using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    public class LocationRenderer
    {
        private Tilemap m_tilemap;
        private TileBase m_floorTile;

        public LocationRenderer(in Tilemap tilemap, in TileBase floorTile)
        {
            m_tilemap = tilemap;
            m_floorTile = floorTile;
        }

        public void DrawTiles(IEnumerable<Vector2Int> floorPositions)
        {
            foreach (Vector2Int floorPosition in floorPositions)
            {
                Vector3Int tilePosition = m_tilemap.WorldToCell((Vector3Int)floorPosition);
                m_tilemap.SetTile(tilePosition, m_floorTile);
            }
        }

        public void Clear()
        {
            m_tilemap.ClearAllTiles();
        }
    }
}