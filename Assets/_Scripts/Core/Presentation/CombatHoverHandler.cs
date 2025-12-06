using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;

namespace MagmaHeart.Core.Presentation
{
    public class CombatHoverHandler : IHoverHandler<CombatHoverResult>
    {
        private readonly PlayerTurnContext m_turnContext;
        private RoomTile m_currentTile;
        private Entity m_currentEntity;

        private Room Room => m_turnContext.CurrentRoom;

        public CombatHoverHandler(PlayerTurnContext playerTurnContext) => m_turnContext = playerTurnContext;

        public void HandleHoverResult(CombatHoverResult hoverResult)
        {
            if (m_currentTile == hoverResult.RoomTile)
                return;

            if (m_currentEntity != null && hoverResult.Entity != m_currentEntity)
                m_currentEntity.Outline.RemoveOutline();

            if (m_currentTile != null && hoverResult.RoomTile != m_currentTile)
                Room?.HideCombatTileAt(m_currentTile);

            m_currentEntity = hoverResult.Entity;
            m_currentTile = hoverResult.RoomTile;

            if (m_currentTile == null)
                return;

            UnitAction possibleAction = m_turnContext.SelectAction(m_currentTile);

            if (possibleAction == null && m_currentEntity != null)
            {
                if (m_currentEntity.Model.IsPlayer)
                    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ALLY_OUTLINE);
                else
                    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ENEMY_OUTLINE);
            }
            else if (possibleAction is MovementAction)
            {
                Room.TryDisplayCombatTile(m_currentTile);
            }
            else if (possibleAction is AttackAction && m_currentEntity != null)
            {
                m_currentEntity.Outline.ApplyOutline(OutlineSettings.CAN_ATTACK_OUTLINE);
            }
        }

        public void ClearHover()
        {
            m_currentEntity?.Outline.RemoveOutline();

            if (m_currentTile != null)
                Room?.HideCombatTileAt(m_currentTile);

            m_currentEntity = null;
            m_currentTile = null;
        }

        public void HandleHoverResult(HoverResult hoverResult) => HandleHoverResult((CombatHoverResult)hoverResult);
    }
}