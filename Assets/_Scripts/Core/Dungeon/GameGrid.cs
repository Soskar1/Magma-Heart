using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.Dungeon
{
    [RequireComponent(typeof(Grid))]
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap m_floor;
        [SerializeField] private Tilemap m_walls;
        [SerializeField] private Tilemap m_corridors;

        [SerializeField] private TileBase m_floorTile;
        [SerializeField] private TileBase m_wallTile;

        private Grid m_grid;
        private Vector3 m_offsetToTileCenter;

        public Tilemap Floor => m_floor;
        public Tilemap Walls => m_walls;
        public Tilemap Corridors => m_corridors;

        public TileBase FloorTile => m_floorTile;
        public TileBase WallTile => m_wallTile;

        public void Initialize()
        {
            m_grid = GetComponent<Grid>();
            m_offsetToTileCenter = m_grid.cellSize / 2;
        }

        // If you want to get a proper tile position through world position you need to use this method
        // Problem is that if you try to use another tilemap that not placed on the same position as m_floor/m_walls/m_corridors tilemaps,
        // then you will receive offsetted tile positions
        public Vector3Int WorldToTilePosition(Vector2 worldPosition) => m_floor.WorldToCell(worldPosition);

        public Vector2 TilePositionToWorld(Vector3Int roomTilePosition) => m_floor.CellToWorld(roomTilePosition);

        public Vector3 WorldToTileCenterPosition(Vector3 worldPosition)
        {
            Vector3Int tilePosition = WorldToTilePosition(worldPosition);
            return tilePosition + m_offsetToTileCenter;
        }

        public Vector3 ToTileCenter(Vector3Int tile)
        {
            return tile + m_offsetToTileCenter;
        }

        public int DistanceBetweenTilesInWorld(Vector2 worldPosition1, Vector2 worldPosition2)
        {
            Vector3Int tile1 = WorldToTilePosition(worldPosition1);
            Vector3Int tile2 = WorldToTilePosition(worldPosition2);
            return ManhattanDistance(tile1, tile2);
        }

        public int ManhattanDistance(Vector3Int tile1, Vector3Int tile2) => Mathf.Abs(tile1.x - tile2.x) + Mathf.Abs(tile1.y - tile2.y);
    }
}