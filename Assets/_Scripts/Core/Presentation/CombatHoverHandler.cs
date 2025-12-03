using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class CombatHoverHandler : IHoverHandler
    {
        private readonly Battle m_battle;
        private readonly PlayerCombatController m_controller;
        private CombatBoardState m_currentBoard;
        private RoomTile m_currentTile;

        public CombatHoverHandler(Battle battle, PlayerCombatController playerCombatController)
        {
            m_battle = battle;
            m_controller = playerCombatController;
            m_battle.OnBattleStarted += HandleOnBattleStarted;
        }

        public void Disable() => m_battle.OnBattleStarted -= HandleOnBattleStarted;

        public void HandleHover(Vector2 worldPosition)
        {
            Vector3Int tilePosition = m_currentBoard.Room.Grid.WorldToTilePosition(worldPosition);
            RoomTile hoveredTile = m_currentBoard.Room.GetRoomTile(tilePosition);

            if (m_currentTile == hoveredTile)
                return;

            m_currentTile = hoveredTile;
            m_currentBoard.Room.HideCombatTileAt(m_currentTile);

            if (hoveredTile != null)
            {
                UnitAction possibleAction = m_controller.SelectAction(hoveredTile);
                // TODO: highlight strategy
                //hoveredTile.HighlightForAction(possibleAction);
            }
        }

        private void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args) => m_currentBoard = args.CombatBoardState; 
    }
}