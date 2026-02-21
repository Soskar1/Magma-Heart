using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomTile
    {
        private readonly WorldGrid m_grid;
        public Vector3Int Position { get; init; }

        public Vector2 TileCenter => m_grid.ToTileCenter(Position.ToVector2Int());
        public RoomTile(Vector3Int position, WorldGrid grid)
        {
            Position = position;
            m_grid = grid;
        }
    }
}
