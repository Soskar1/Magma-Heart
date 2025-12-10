using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Entities.Presenters;

namespace MagmaHeart.Core.Presentation
{
    public class CombatHoverHandler : IHoverHandler
    {
        private readonly PlayerTurnContext m_turnContext;
        private RoomTile m_currentTile;
        private Entity m_currentEntity;

        private Room Room => m_turnContext.CurrentRoom;

        public CombatHoverHandler(PlayerTurnContext playerTurnContext) => m_turnContext = playerTurnContext;

        public void Visit(CombatHoverResult result)
        {
            if (m_currentTile == result.RoomTile)
                return;

            if (m_currentEntity != null && result.Entity != m_currentEntity)
                m_currentEntity.Outline.RemoveOutline();

            if (m_currentTile != null && result.RoomTile != m_currentTile)
                Room?.HideCombatTileAt(m_currentTile);

            m_currentEntity = result.Entity;
            m_currentTile = result.RoomTile;

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

        public void Visit(UIHoverResult result)
        {
            if (result.UIElement == null)
                return;

            EntityPresenter presenter = result.UIElement.GetComponent<EntityPresenter>();

            if (presenter == null)
                return;

            if (m_currentEntity != null && presenter.Model != m_currentEntity.Model)
                m_currentEntity.Outline.RemoveOutline();

            //m_currentEntity = presenter.Model;

            //if (m_currentEntity.Model.IsPlayer)
            //    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ALLY_OUTLINE);
            //else
            //    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ENEMY_OUTLINE);
        }

        public void Visit(RaycastHoverResult result) { }

        public void ClearHover()
        {
            m_currentEntity?.Outline.RemoveOutline();

            if (m_currentTile != null)
                Room?.HideCombatTileAt(m_currentTile);

            m_currentEntity = null;
            m_currentTile = null;
        }        
    }
}