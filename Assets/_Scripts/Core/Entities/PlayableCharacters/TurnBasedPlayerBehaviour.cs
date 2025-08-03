using System;
using System.Collections.Generic;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedPlayerBehaviour : IPlayerBehaviour, ICombatController
    {
        private UserInput m_userInput;
        private MouseControl m_mouseControl;
        private List<IDisplayable> m_combatUI;
        private Energy m_energy;
        private Room m_currentRoom;

        private Transform m_playerTransform;
        private Vector3Int m_playersCurrentTile;

        private Vector3Int? m_previousMousePosition;

        public Action NextTurn { get; set; }

        private bool m_playerTurnIsActive;

        public TurnBasedPlayerBehaviour(Transform playerTransform, Energy energy, UserInput userInput, MouseControl mouseControl, List<IDisplayable> combatUI)
        {
            m_playerTransform = playerTransform;
            m_energy = energy;
            m_userInput = userInput;
            m_mouseControl = mouseControl;
            m_combatUI = combatUI;
            m_playerTurnIsActive = false;
        }

        public void Enable()
        {
            m_userInput.Controls.TurnBasedPlayer.Enable();
            m_mouseControl.OnMouseChangedTile += DisplayCombatTile;
            m_mouseControl.OnMouseChangedTile += DisplayEnergyForMovementAction;
        }

        public void Disable()
        {
            m_userInput.Controls.TurnBasedPlayer.Disable();
            m_mouseControl.OnMouseChangedTile -= DisplayCombatTile;
            m_mouseControl.OnMouseChangedTile -= DisplayEnergyForMovementAction;
        }

        public void Update()
        {
            if (!m_playerTurnIsActive)
                return;

            m_mouseControl.UpdateMousePosition();
        }
        
        private void DisplayCombatTile(Vector3Int mouseRoomTilePosition)
        {
            if (m_previousMousePosition.HasValue)
                m_currentRoom.HideCombatTileAt(m_previousMousePosition.Value);

            m_currentRoom.TryDisplayCombatTile(mouseRoomTilePosition);
            m_previousMousePosition = mouseRoomTilePosition;
        }

        private void DisplayEnergyForMovementAction(Vector3Int mouseRoomTilePosition)
        {
            int distance = m_currentRoom.Grid.ManhattanDistance(m_playersCurrentTile, mouseRoomTilePosition);
            Debug.Log($"Distance between {m_playersCurrentTile} and {mouseRoomTilePosition} is {distance} tiles");
        }

        public void StartCombat(Room room)
        {
            m_currentRoom = room;

            // Move player at the center of the current standing tile
            m_playersCurrentTile = m_currentRoom.Grid.WorldToTilePosition(m_playerTransform.position);
            m_playerTransform.position = m_currentRoom.Grid.ToTileCenter(m_playersCurrentTile);
        }

        public void StartTurn()
        {
            m_energy.Regenerate();

            Debug.Log("Player is doing a move");
            foreach (IDisplayable ui in m_combatUI)
                ui.Show();

            m_playerTurnIsActive = true;
        }

        public void EndTurn()
        {
            Debug.Log("Player ended his move");
            m_playerTurnIsActive = false;
            m_previousMousePosition = null;
            if (m_mouseControl.CurrentMouseTile.HasValue)
            {
                m_currentRoom.HideCombatTileAt(m_mouseControl.CurrentMouseTile.Value);
                m_mouseControl.ClearMousePosition();
            }

            foreach (IDisplayable ui in m_combatUI)
                ui.Hide();

            NextTurn?.Invoke();
        }
    }
}