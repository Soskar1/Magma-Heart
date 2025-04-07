using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public enum TileType
    {
        Floor,
        Wall
    }

    public class DungeonTile
    {
        public Vector2Int Position { get; private set; }
        public TileType TileType { get; private set; }

        public DungeonTile(Vector2Int position, TileType type)
        {
            Position = position;
            TileType = type;
        }
    }
}