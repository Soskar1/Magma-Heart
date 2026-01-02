using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public class TileMouseHoverStrategy : MouseHoverStrategy
    {
        private readonly DungeonController m_dungeonController;

        public TileMouseHoverStrategy(DungeonController dungeonController)
        {
            m_dungeonController = dungeonController;
        }

        protected override HoverResult TryHover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = m_dungeonController.CurrentRoom.Grid.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = m_dungeonController.CurrentRoom.GetRoomTile(tilePosition);
            m_dungeonController.CurrentRoom.TryGetEntity(hoveredTile, out Entity entity);

            return new CombatHoverResult(entity, hoveredTile);
        }
    }
}
