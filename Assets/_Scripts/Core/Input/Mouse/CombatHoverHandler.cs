using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Preview;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Presentation;
using UnityEngine;

namespace MagmaHeart.Core.Input.Mouse
{
    public class CombatHoverHandler : IHoverHandler
    {
        private readonly DungeonController m_dungeonController;
        private readonly ICombatTileHighlighter m_tileHighlighter;
        private readonly IActionPreviewProvider m_actionPreviewProvider;
        private RoomTile m_currentTile;
        private Entity m_currentEntity;

        public CombatHoverHandler(DungeonController dungeonController, IActionPreviewProvider actionPreviewProvider, ICombatTileHighlighter tileHighlighter)
        {
            m_dungeonController = dungeonController;
            m_actionPreviewProvider = actionPreviewProvider;
            m_tileHighlighter = tileHighlighter;
        }

        public void Visit(CombatHoverResult result)
        {
            if (m_currentEntity != null && result.Entity != m_currentEntity)
                m_currentEntity.Outline.RemoveOutline();

            if (result.RoomTile == null)
                return;

            if (m_currentTile != null && m_currentTile.Position == result.RoomTile.Position)
                return;

            m_currentEntity = result.Entity;
            m_currentTile = result.RoomTile;

            if (m_currentEntity != null)
            {
                m_currentEntity.Outline.ApplyOutline(m_currentEntity.Model.IsPlayer
                    ? OutlineSettings.ALLY_OUTLINE : OutlineSettings.ENEMY_OUTLINE);
            }

            m_actionPreviewProvider.Preview(m_currentTile);
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
                m_tileHighlighter.Hide(m_currentTile);

            m_currentTile = null;

            if (!m_dungeonController.CurrentRoom.TryGetEntity(presenter.Model, out Entity entity))
                return;

            m_currentEntity = entity;
            m_currentEntity.Outline.ApplyOutline(m_currentEntity.Model.IsPlayer
                    ? OutlineSettings.ALLY_OUTLINE : OutlineSettings.ENEMY_OUTLINE);
        }

        public void Visit(RaycastHoverResult result) { }

        public void ClearHover()
        {
            m_currentEntity?.Outline.RemoveOutline();

            if (m_currentTile != null)
                m_tileHighlighter.Hide(m_currentTile);

            m_currentEntity = null;
            m_currentTile = null;
        }        
    }
}