using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class CombatHoverHandler : IHoverHandler
    {
        private readonly PlayerTurnContext m_turnContext;
        private RoomTile m_currentTile;
        private Entity m_currentEntity;

        private Room Room => m_turnContext.CurrentRoom;

        public CombatHoverHandler(PlayerTurnContext playerTurnContext) => m_turnContext = playerTurnContext;

        public void HandleHover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = Room.Grid.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = Room.GetRoomTile(tilePosition);
            Room.TryGetEntity(hoveredTile, out Entity entity);

            if (m_currentTile == hoveredTile)
                return;

            if (m_currentEntity != entity)
                m_currentEntity?.Outline.RemoveOutline();

            if (m_currentTile != null)
                Room.HideCombatTileAt(m_currentTile);
            
            m_currentTile = hoveredTile;
            m_currentEntity = entity;

            if (hoveredTile != null)
                HandleHover();
        }

        private void HandleHover()
        {
            UnitAction possibleAction = m_turnContext.SelectAction(m_currentTile);

            // TODO: use strategy pattern
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
    }
}