using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomTileData
    {
        private TileData m_tileData;
        public int TileCount => m_tileData.TileCount;

        private readonly BoundsInt m_roomSpace;
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

        public RoomTileData(in BoundsInt roomSpace, in Vector2Int borderOffsets)
        {
            m_roomSpace = roomSpace;
            WorldPosition = new Vector2Int((int)roomSpace.center.x, (int)roomSpace.center.y);
            RightBorder = m_roomSpace.xMax - 1 - borderOffsets.x;
            TopBorder = m_roomSpace.yMax - 1 - borderOffsets.y;
            LeftBorder = m_roomSpace.xMin + borderOffsets.x;
            BottomBorder = m_roomSpace.yMin + borderOffsets.y;

            m_tileData = new TileData();
            DungeonTile initialTile = new DungeonTile(WorldPosition, TileType.Floor);
            m_tileData.AddTile(initialTile);

            LeftMostTile = WorldPosition;
            RightMostTile = WorldPosition;
            TopMostTile = WorldPosition;
            BottomMostTile = WorldPosition;
        }

        public RoomTileData(in BoundsInt roomSpace) : this(roomSpace, Vector2Int.zero) { }

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

        public void AddTile(Vector2Int tilePosition, TileType tileType) 
        {
            tilePosition = ToRoomSpace(tilePosition);
            DungeonTile dungeonTile = new DungeonTile(tilePosition, tileType);

            m_tileData.AddTile(dungeonTile);

            if (tilePosition.x < LeftMostTile.x)
                LeftMostTile = tilePosition;

            if (tilePosition.x > RightMostTile.x)
                RightMostTile = tilePosition;
            
            if (tilePosition.y > TopMostTile.y)
                TopMostTile = tilePosition;
            
            if (tilePosition.y < BottomMostTile.y)
                BottomMostTile = tilePosition;
        }

        public void AddTiles(in ICollection<Vector2Int> tilesPositions, TileType tileType)
        {
            foreach (Vector2Int tilePosition in tilesPositions)
                AddTile(tilePosition, tileType);
        }

        public void SetTiles(in Dictionary<Vector2Int, DungeonTile> tiles)
        {
            m_tileData.SetTiles(tiles);

            LeftMostTile = tiles.Keys.Aggregate((min, next) => next.x < min.x ? next : min);
            RightMostTile = tiles.Keys.Aggregate((max, next) => next.x > max.x ? next : max);
            BottomMostTile = tiles.Keys.Aggregate((min, next) => next.y < min.y ? next : min);
            TopMostTile = tiles.Keys.Aggregate((max, next) => next.y > max.y ? next : max);
        }

        public DungeonTile GetTile(Vector2Int position) => m_tileData.GetTile(position);
        public DungeonTile GetTileAtIndex(in int index) => m_tileData.GetTileAtIndex(index);
        public HashSet<DungeonTile> GetTiles() => m_tileData.GetTiles();
        public HashSet<Vector2Int> GetTilePositions() => m_tileData.GetTilePositions();
        public bool ContainsTileAtPosition(Vector2Int position) => m_tileData.ContainsTileAtPosition(position);
        public Vector2Int GetTilePositionAtIndex(in int index) => GetTileAtIndex(index).Position;
    }
}