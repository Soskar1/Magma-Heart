using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomTile
    {
        private readonly RoomGrid m_grid;
        public Vector3Int Position { get; init; }

        public Vector2 TileCenter => m_grid.ToTileCenter(Position.ToVector2Int());
        public RoomTile(Vector3Int position, RoomGrid grid)
        {
            Position = position;
            m_grid = grid;
        }
    }
}
