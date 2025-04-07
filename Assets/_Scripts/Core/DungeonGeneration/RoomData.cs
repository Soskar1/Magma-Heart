using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomData
    {
        private Dictionary<Vector2Int, DungeonTile> m_tiles;
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

            DungeonTile initialTile = new DungeonTile(WorldPosition, TileType.Floor);
            m_tiles = new Dictionary<Vector2Int, DungeonTile>();
            m_tiles.Add(WorldPosition, initialTile);

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

        public void AddTile(Vector2Int tilePosition, TileType tileType) 
        {
            tilePosition = ToRoomSpace(tilePosition);
            DungeonTile dungeonTile = new DungeonTile(tilePosition, tileType);

            if (m_tiles.ContainsKey(tilePosition))
            {
                if (m_tiles[tilePosition].TileType != tileType)
                    m_tiles[tilePosition].TileType = dungeonTile.TileType;

                return;
            }

            m_tiles.Add(tilePosition, dungeonTile);

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
            m_tiles = tiles;

            LeftMostTile = tiles.Keys.Aggregate((min, next) => next.x < min.x ? next : min);
            RightMostTile = tiles.Keys.Aggregate((max, next) => next.x > max.x ? next : max);
            BottomMostTile = tiles.Keys.Aggregate((min, next) => next.y < min.y ? next : min);
            TopMostTile = tiles.Keys.Aggregate((max, next) => next.y > max.y ? next : max);
        }

        public DungeonTile GetTile(in Vector2Int tilePosition)
        {
            if (m_tiles.ContainsKey(tilePosition))
                return m_tiles[tilePosition];

            return null;
        }

        public DungeonTile GetTileAtIndex(in int index) => m_tiles.Values.ElementAt(index);

        public Vector2Int GetTilePositionAtIndex(in int index) => GetTileAtIndex(index).Position;

        public bool ContainsTileAtPosition(Vector2Int tileToFind) => m_tiles.ContainsKey(tileToFind);
 
        public Dictionary<Vector2Int, DungeonTile> GetTilesCopy() => new Dictionary<Vector2Int, DungeonTile>(m_tiles);

        public HashSet<Vector2Int> GetTilePositions() => m_tiles.Keys.ToHashSet();

        public HashSet<DungeonTile> GetTiles() => m_tiles.Values.ToHashSet();
    }
}