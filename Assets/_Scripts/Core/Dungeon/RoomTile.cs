using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class RoomTile
    {
        private Room m_room;
        public Vector3Int Position { get; private set; }
        public Vector2 TileCenter => m_room.Grid.ToTileCenter(Position.ToVector2Int());
        public RoomTile(Room room, Vector3Int position)
        {
            m_room = room;
            Position = position;
        }

        public CombatTile ToCombatTile()
        {
            Vector2 worldPosition = m_room.Grid.TilePositionToWorld(Position);
            Vector3Int combatTilePosition = m_room.CombatTilemap.WorldToCell(worldPosition);
            return new CombatTile(combatTilePosition);
        }
    }
}
