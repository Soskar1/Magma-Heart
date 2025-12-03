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
        private Room Room => m_turnContext.CurrentRoom;

        public CombatHoverHandler(PlayerTurnContext playerTurnContext)
        {
            m_turnContext = playerTurnContext;
        }

        public void HandleHover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = Room.Grid.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = Room.GetRoomTile(tilePosition);

            if (m_currentTile == hoveredTile)
                return;

            if (m_currentTile != null)
                Room.HideCombatTileAt(m_currentTile);
            
            m_currentTile = hoveredTile;

            if (hoveredTile != null)
            {
                UnitAction possibleAction = m_turnContext.SelectAction(hoveredTile);

                // TODO: use strategy pattern
                if (possibleAction == null)
                {
                    if (Room.EntityIsOnTile(hoveredTile, out EntityModel model))
                    {
                        if (model.IsPlayer)
                        {
                            // Green outline
                        }
                        else
                        {
                            // No attack outline
                        }
                    }
                }
                else if (possibleAction is MovementAction)
                {
                    Room.TryDisplayCombatTile(hoveredTile);
                }
                else if (possibleAction is AttackAction)
                {
                    // Attack outline
                }
            }
        }
    }
}