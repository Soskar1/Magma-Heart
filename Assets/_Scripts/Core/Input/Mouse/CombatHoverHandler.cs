using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.Input.Mouse
{
    public class CombatHoverHandler : IHoverHandler
    {
        private readonly Battle m_battle;
        private readonly IActionPreviewProvider m_actionPreviewProvider;
        private RoomTile m_currentTile;
        private Entity m_currentEntity;

        public CombatHoverHandler(Battle battle, IActionPreviewProvider actionPreviewProvider)
        {
            m_battle = battle;
            m_actionPreviewProvider = actionPreviewProvider;
        }

        public void Visit(CombatHoverResult result)
        {
            if (m_currentTile == result.RoomTile)
                return;

            if (m_currentEntity != null && result.Entity != m_currentEntity)
                m_currentEntity.Outline.RemoveOutline();

            if (m_currentTile != null && result.RoomTile != m_currentTile)
                m_battle.CurrentRoom?.HideCombatTileAt(m_currentTile);

            m_currentEntity = result.Entity;
            m_currentTile = result.RoomTile;

            if (m_currentTile == null)
                return;

            ActionPreview actionPreview = m_actionPreviewProvider.Preview(m_currentTile);

            if (actionPreview == null)
            {
                if (m_currentEntity != null)
                {
                    if (m_currentEntity.Model.IsPlayer)
                        m_currentEntity.Outline.ApplyOutline(OutlineSettings.ALLY_OUTLINE);
                    else
                        m_currentEntity.Outline.ApplyOutline(OutlineSettings.ENEMY_OUTLINE);
                }
            }
            else if (actionPreview.Action is MovementAction)
            {
                m_battle.CurrentRoom.TryDisplayCombatTile(m_currentTile);
            }
            else if (actionPreview.Action is AttackAction && m_currentEntity != null)
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

            if (m_currentTile != null)
                m_battle.CurrentRoom?.HideCombatTileAt(m_currentTile);

            m_currentTile = null;

            if (!m_battle.CurrentRoom.TryGetEntity(presenter.Model, out Entity entity))
                return;

            m_currentEntity = entity;

            if (m_currentEntity.Model.IsPlayer)
                m_currentEntity.Outline.ApplyOutline(OutlineSettings.ALLY_OUTLINE);
            else
                m_currentEntity.Outline.ApplyOutline(OutlineSettings.ENEMY_OUTLINE);
        }

        public void Visit(RaycastHoverResult result) { }

        public void ClearHover()
        {
            m_currentEntity?.Outline.RemoveOutline();

            if (m_currentTile != null)
                m_battle.CurrentRoom?.HideCombatTileAt(m_currentTile);

            m_currentEntity = null;
            m_currentTile = null;
        }        
    }
}