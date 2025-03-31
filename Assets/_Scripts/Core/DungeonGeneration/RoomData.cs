using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomData
    {
        private HashSet<Vector2Int> m_tiles;
        private readonly BoundsInt m_roomSpace;
        public int TileCount => m_tiles.Count;
        public BoundsInt RoomSpace => m_roomSpace;
        public Vector2Int WorldPosition { get; private set; }
        public int LeftBorder { get; private set; }
        public int RightBorder { get; private set; }
        public int TopBorder { get; private set; }
        public int BottomBorder { get; private set; }

        public Vector2Int LeftMostTile { get; private set; }
        public Vector2Int RightMostTile { get; private set; }
        public Vector2Int TopMostTile { get; private set; }
        public Vector2Int BottomMostTile { get; private set; }

        public RoomData(in BoundsInt roomSpace, in Vector2Int borderOffsets)
        {
            m_roomSpace = roomSpace;
            WorldPosition = new Vector2Int((int)roomSpace.center.x, (int)roomSpace.center.y);
            RightBorder = m_roomSpace.xMax - 1 - borderOffsets.x;
            TopBorder = m_roomSpace.yMax - 1 - borderOffsets.y;
            LeftBorder = m_roomSpace.xMin + borderOffsets.x;
            BottomBorder = m_roomSpace.yMin + borderOffsets.y;

            m_tiles = new HashSet<Vector2Int>() { WorldPosition };
            LeftMostTile = WorldPosition;
            RightMostTile = WorldPosition;
            TopMostTile = WorldPosition;
            BottomMostTile = WorldPosition;
        }

        public RoomData(in BoundsInt roomSpace) : this(roomSpace, Vector2Int.zero) { }

        public Vector2Int ToRoomSpace(Vector2Int position)
        {
            if (position.x > RightBorder)
                position.x = RightBorder;

            if (position.x < LeftBorder)
                position.x = LeftBorder;

            if (position.y > TopBorder)
                position.y = TopBorder;

            if (position.y < BottomBorder)
                position.y = BottomBorder;
            
            return position;
        }

        public void AddTile(Vector2Int tile) 
        {
            tile = ToRoomSpace(tile);
            m_tiles.Add(tile);

            if (tile.x < LeftMostTile.x)
                LeftMostTile = tile;

            if (tile.x > RightMostTile.x)
                RightMostTile = tile;
            
            if (tile.y > TopMostTile.y)
                TopMostTile = tile;
            
            if (tile.y < BottomMostTile.y)
                BottomMostTile = tile;
        }

        public void AddTiles(in ICollection<Vector2Int> tiles)
        {
            foreach (Vector2Int tile in tiles)
                AddTile(tile);
        }

        public void SetTiles(in HashSet<Vector2Int> tiles)
        {
            m_tiles = tiles;

            LeftMostTile = tiles.Aggregate((min, next) => next.x < min.x ? next : min);
            RightMostTile = tiles.Aggregate((max, next) => next.x > max.x ? next : max);
            BottomMostTile = tiles.Aggregate((min, next) => next.y < min.y ? next : min);
            TopMostTile = tiles.Aggregate((max, next) => next.y > max.y ? next : max);
        }

        public Vector2Int GetTileAtIndex(in int index) => m_tiles.ElementAt(index);

        public bool ContainsTile(in Vector2Int tile) => m_tiles.Contains(tile);
 
        public HashSet<Vector2Int> GetTilesCopy() => new HashSet<Vector2Int>(m_tiles);
    }
}