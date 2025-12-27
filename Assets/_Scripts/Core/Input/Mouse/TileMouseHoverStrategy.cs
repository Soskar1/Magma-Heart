using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public class TileMouseHoverStrategy : MouseHoverStrategy
    {
        private readonly Battle m_battle;

        public TileMouseHoverStrategy(Battle battle)
        {
            m_battle = battle;
        }

        protected override HoverResult TryHover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = m_battle.CurrentRoom.Grid.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = m_battle.CurrentRoom.GetRoomTile(tilePosition);
            m_battle.CurrentRoom.TryGetEntity(hoveredTile, out Entity entity);

            return new CombatHoverResult(entity, hoveredTile);
        }
    }
}
