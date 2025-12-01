using MagmaHeart.Core.BoardStateSystem;
using UnityEngine;
using MagmaHeart.Core.Entities.PlayableCharacters;

namespace MagmaHeart.Core.Presentation
{
    public class CombatHoverHandler : IHoverHandler
    {
        private readonly CombatBoardState m_combatBoardState;
        private readonly ActionSelector m_actionSelectorChain;
        private RoomTile m_currentTile;

        public CombatHoverHandler(CombatBoardState combatBoardState, ActionSelector actionSelector)
        {
            m_combatBoardState = combatBoardState;
            m_actionSelectorChain = actionSelector;
        }

        public void HandleHover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = m_combatBoardState.Room.Grid.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = m_combatBoardState.Room.GetRoomTile(tilePosition);

            if (m_currentTile == hoveredTile)
                return;

            m_combatBoardState.Room.HideCombatTileAt(m_currentTile);
            m_currentTile = hoveredTile;

            if (hoveredTile != null)
            {
                //var possibleAction = m_logic.GetAvailableActionOnTile(hoveredTile);
                //hoveredTile.HighlightForAction(possibleAction);
            }
        }
    }
}