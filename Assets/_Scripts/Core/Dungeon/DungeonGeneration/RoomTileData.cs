using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomTileData : IEnumerable<DungeonTile>
    {
        public TileData TileData { get; private set; }
        public int TileCount => TileData.TileCount;

        private readonly BoundsInt m_roomSpace;
        public BoundsInt RoomSpace => m_roomSpace;
        public Vector2Int WorldPosition { get; private set; }
        public int LeftBorder { get; private set; }
        public int RightBorder { get; private set; }
        public int TopBorder { get; private set; }
        public int BottomBorder { get; private set; }

        public RoomTileData(in BoundsInt roomSpace, in Vector2Int borderOffsets)
        {
            m_roomSpace = roomSpace;
            WorldPosition = new Vector2Int((int)roomSpace.center.x, (int)roomSpace.center.y);
            RightBorder = m_roomSpace.xMax - 1 - borderOffsets.x;
            TopBorder = m_roomSpace.yMax - 1 - borderOffsets.y;
            LeftBorder = m_roomSpace.xMin + borderOffsets.x;
            BottomBorder = m_roomSpace.yMin + borderOffsets.y;

            TileData = new TileData();
            DungeonTile initialTile = new DungeonTile(WorldPosition, TileType.Floor);
            TileData.AddTile(initialTile);
        }

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

            TileData.AddTile(dungeonTile);
        }

        public void AddTiles(in ICollection<Vector2Int> tilesPositions, TileType tileType)
        {
            foreach (Vector2Int tilePosition in tilesPositions)
                AddTile(tilePosition, tileType);
        }

        public DungeonTile GetTile(Vector2Int position) => TileData.GetTile(position);
        public DungeonTile GetTileAtIndex(in int index) => TileData.GetTileAtIndex(index);
        public HashSet<DungeonTile> GetTiles() => TileData.GetTiles();
        public IEnumerator<DungeonTile> GetEnumerator() => TileData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => TileData.GetEnumerator();
        public HashSet<Vector2Int> GetTilePositions() => TileData.GetTilePositions();
        public bool ContainsTileAtPosition(Vector2Int position) => TileData.ContainsTileAtPosition(position);
        public Vector2Int GetTilePositionAtIndex(in int index) => GetTileAtIndex(index).Position;
        public IEnumerable<DungeonTile> GetAdjacentTiles(Vector2Int sourceTile) => TileData.GetAdjacentTiles(sourceTile);
        public IEnumerable<DungeonTile> GetAdjacentTiles(DungeonTile sourceTile) => GetAdjacentTiles(sourceTile.Position);
    }
}