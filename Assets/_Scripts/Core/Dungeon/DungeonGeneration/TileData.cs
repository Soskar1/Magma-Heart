using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class TileData
    {
        private Dictionary<Vector2Int, DungeonTile> m_tiles;
        public int TileCount => m_tiles.Count;

        public Vector2Int LeftMostTile { get; private set; }
        public Vector2Int RightMostTile { get; private set; }
        public Vector2Int TopMostTile { get; private set; }
        public Vector2Int BottomMostTile { get; private set; }

        public TileData()
        {
            m_tiles = new Dictionary<Vector2Int, DungeonTile>();
            LeftMostTile = new Vector2Int(0, 0);
            RightMostTile = new Vector2Int(0, 0);
            TopMostTile = new Vector2Int(0, 0);
            BottomMostTile = new Vector2Int(0, 0);
        }

        public void AddTile(DungeonTile tile)
        {
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

        public DungeonTile GetTile(in Vector2Int tilePosition)
        {
            if (m_tiles.ContainsKey(tilePosition))
                return m_tiles[tilePosition];

            return null;
        }

        public HashSet<DungeonTile> GetTiles() => m_tiles.Values.ToHashSet();
        public HashSet<Vector2Int> GetTilePositions() => m_tiles.Keys.ToHashSet();
        
        public void SetTiles(in Dictionary<Vector2Int, DungeonTile> tiles)
        {
            m_tiles = tiles;

            LeftMostTile = tiles.Keys.Aggregate((min, next) => next.x < min.x ? next : min);
            RightMostTile = tiles.Keys.Aggregate((max, next) => next.x > max.x ? next : max);
            BottomMostTile = tiles.Keys.Aggregate((min, next) => next.y < min.y ? next : min);
            TopMostTile = tiles.Keys.Aggregate((max, next) => next.y > max.y ? next : max);
        }

        public DungeonTile GetTileAtIndex(in int index) => m_tiles.Values.ElementAt(index);

        public bool ContainsTileAtPosition(Vector2Int tileToFind) => m_tiles.ContainsKey(tileToFind);
    }
}