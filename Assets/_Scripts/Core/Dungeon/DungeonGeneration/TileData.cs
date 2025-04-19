using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class TileData
    {
        private Dictionary<Vector2Int, DungeonTile> m_tiles;
        public int TileCount => m_tiles.Count;

        public TileData() => m_tiles = new Dictionary<Vector2Int, DungeonTile>();

        public void AddTile(DungeonTile tile)
        {
            if (ContainsTileAtPosition(tile.Position))
                m_tiles[tile.Position].Type = tile.Type;
            else
                m_tiles.Add(tile.Position, tile);
        }

        public DungeonTile GetTile(in Vector2Int tilePosition)
        {
            if (m_tiles.ContainsKey(tilePosition))
                return m_tiles[tilePosition];

            return null;
        }

        public HashSet<DungeonTile> GetTiles() => m_tiles.Values.ToHashSet();
        public HashSet<Vector2Int> GetTilePositions() => m_tiles.Keys.ToHashSet();
        public void SetTiles(in Dictionary<Vector2Int, DungeonTile> tiles) => m_tiles = tiles;
        public DungeonTile GetTileAtIndex(in int index) => m_tiles.Values.ElementAt(index);

        public bool ContainsTileAtPosition(Vector2Int tileToFind) => m_tiles.ContainsKey(tileToFind);
    }
}