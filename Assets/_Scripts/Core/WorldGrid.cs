using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core
{
    public class WorldGrid
    {
        private readonly Grid m_grid;
        private readonly Tilemap m_tilemap;
        private Vector3 m_offsetToTileCenter;

        public WorldGrid(Grid grid, Tilemap tilemap)
        {
            m_grid = grid;
            m_tilemap = tilemap;
            m_offsetToTileCenter = m_grid.cellSize / 2;
        }

        // If you want to get a proper tile position through world position you need to use this method
        // Problem is that if you try to use another tilemap that not placed on the same position as m_floor/m_walls/m_corridors tilemaps,
        // then you will receive offsetted tile positions
        public Vector3Int WorldToTilePosition(Vector2 worldPosition) => m_tilemap.WorldToCell(worldPosition);

        public Vector2 ToTileCenter(Vector2Int tile) => tile.ToVector2() + (Vector2)m_offsetToTileCenter;

        public static int ManhattanDistance(Vector3Int tile1, Vector3Int tile2) => Mathf.Abs(tile1.x - tile2.x) + Mathf.Abs(tile1.y - tile2.y);
    }
}