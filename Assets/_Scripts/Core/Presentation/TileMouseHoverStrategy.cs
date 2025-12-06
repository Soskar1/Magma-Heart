using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class TileMouseHoverStrategy : IMouseHoverStrategy
    {
        private readonly PlayerTurnContext m_turnContext;

        private Room Room => m_turnContext.CurrentRoom;

        public TileMouseHoverStrategy(PlayerTurnContext playerTurnContext) => m_turnContext = playerTurnContext;

        public HoverResult Hover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = Room.Grid.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = Room.GetRoomTile(tilePosition);
            Room.TryGetEntity(hoveredTile, out Entity entity);

            return new CombatHoverResult(entity, hoveredTile);
        }
    }
}
