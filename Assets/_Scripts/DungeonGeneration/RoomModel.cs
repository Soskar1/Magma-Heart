using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace MagmaHeart.DungeonGeneration
{
    public class RoomModel : IEnumerable<DungeonTile>
    {
        private Dictionary<Vector2Int, DungeonTile> m_tiles;
        public int TileCount => m_tiles.Count;
        public BoundsInt RoomSpace { get; init; }
        public Vector2Int WorldPosition { get; init; }
        public int LeftBorder { get; init; }
        public int RightBorder { get; init; }
        public int TopBorder { get; init; }
        public int BottomBorder { get; init; }
        public Vector2Int LeftMostTile { get; private set; }
        public Vector2Int RightMostTile { get; private set; }
        public Vector2Int TopMostTile { get; private set; }
        public Vector2Int BottomMostTile { get; private set; }

        public RoomModel(in BoundsInt roomSpace)
        {
            m_tiles = new Dictionary<Vector2Int, DungeonTile>();
            RoomSpace = roomSpace;

            WorldPosition = new Vector2Int((int)roomSpace.center.x, (int)roomSpace.center.y);
            
            RightBorder = RoomSpace.xMax - 1;
            TopBorder = RoomSpace.yMax - 1;
            LeftBorder = RoomSpace.xMin;
            BottomBorder = RoomSpace.yMin;

            LeftMostTile = new Vector2Int(0, 0);
            RightMostTile = new Vector2Int(0, 0);
            TopMostTile = new Vector2Int(0, 0);
            BottomMostTile = new Vector2Int(0, 0);

            AddTile(WorldPosition, TileType.Floor);
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
            DungeonTile tile = new DungeonTile(tilePosition, tileType);

            if (ContainsTileAtPosition(tile.Position))
            {
                m_tiles[tile.Position].Type = tile.Type;
            }
            else
            {
                m_tiles.Add(tile.Position, tile);

                if (tile.Position.x < LeftMostTile.x)
                    LeftMostTile = tile.Position;

                if (tile.Position.x > RightMostTile.x)
                    RightMostTile = tile.Position;

                if (tile.Position.y > TopMostTile.y)
                    TopMostTile = tile.Position;

                if (tile.Position.y < BottomMostTile.y)
                    BottomMostTile = tile.Position;
            }
        }

        public void AddTiles(in ICollection<Vector2Int> tilesPositions, TileType tileType)
        {
            foreach (Vector2Int tilePosition in tilesPositions)
                AddTile(tilePosition, tileType);
        }

        public DungeonTile GetTile(Vector2Int position)
        {
            if (m_tiles.ContainsKey(position))
                return m_tiles[position];

            return null;
        }

        public HashSet<DungeonTile> GetTiles() => m_tiles.Values.ToHashSet();

        public void SetTiles(in Dictionary<Vector2Int, DungeonTile> tiles)
        {
            m_tiles = tiles;

            LeftMostTile = tiles.Keys.Aggregate((min, next) => next.x < min.x ? next : min);
            RightMostTile = tiles.Keys.Aggregate((max, next) => next.x > max.x ? next : max);
            BottomMostTile = tiles.Keys.Aggregate((min, next) => next.y < min.y ? next : min);
            TopMostTile = tiles.Keys.Aggregate((max, next) => next.y > max.y ? next : max);
        }

        public DungeonTile GetTileAtIndex(in int index) => m_tiles.Values.ElementAt(index);
        public IEnumerator<DungeonTile> GetEnumerator() => m_tiles.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public HashSet<Vector2Int> GetTilePositions() => m_tiles.Keys.ToHashSet();
        public bool ContainsTileAtPosition(Vector2Int position) => m_tiles.ContainsKey(position);
        public Vector2Int GetTilePositionAtIndex(in int index) => GetTileAtIndex(index).Position;
        public IEnumerable<DungeonTile> GetAdjacentTiles(Vector2Int sourceTile)
        {
            if (m_tiles.ContainsKey(sourceTile))
            {
                if (m_tiles.ContainsKey(sourceTile + Vector2Int.up))
                    yield return m_tiles[sourceTile + Vector2Int.up];

                if (m_tiles.ContainsKey(sourceTile + Vector2Int.right))
                    yield return m_tiles[sourceTile + Vector2Int.right];

                if (m_tiles.ContainsKey(sourceTile + Vector2Int.down))
                    yield return m_tiles[sourceTile + Vector2Int.down];

                if (m_tiles.ContainsKey(sourceTile + Vector2Int.left))
                    yield return m_tiles[sourceTile + Vector2Int.left];
            }
        }

        public IEnumerable<DungeonTile> GetAdjacentTiles(DungeonTile sourceTile) => GetAdjacentTiles(sourceTile.Position);
    }
}