using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class CombatHoverHandler : IHoverHandler
    {
        //private Tile _current;

        //private readonly Board m_board;
        //private readonly CombatLogic m_logic;

        //public CombatHoverHandler(Board board, CombatLogic logic)
        //{
        //    m_board = board;
        //    m_logic = logic;
        //}

        //public void HandleHover(Vector2 worldPos)
        //{
        //    Tile hoveredTile = m_board.GetTileAt(worldPos);

        //    if (_current == hoveredTile)
        //        return;

        //    _current?.DisableHighlight();
        //    _current = hoveredTile;

        //    if (hoveredTile != null)
        //    {
        //        var possibleAction = m_logic.GetAvailableActionOnTile(hoveredTile);
        //        hoveredTile.HighlightForAction(possibleAction);
        //    }
        //}
        public void HandleHover(Vector2 worldPosition)
        {
            throw new System.NotImplementedException();
        }
    }

}