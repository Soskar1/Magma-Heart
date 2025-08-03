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
        private List<IDisplayable> m_combatUI;
        private Energy m_energy;
        private Room m_currentRoom;
        private Transform m_playerTransform;

        public Action NextTurn { get; set; }

        private bool m_playerTurnIsActive;

        public TurnBasedPlayerBehaviour(Transform playerTransform, Energy energy, UserInput userInput, List<IDisplayable> combatUI)
        {
            m_playerTransform = playerTransform;
            m_energy = energy;
            m_userInput = userInput;
            m_combatUI = combatUI;
            m_playerTurnIsActive = false;
        }

        public void Enable()
        {
            m_userInput.Controls.TurnBasedPlayer.Enable();
        }

        public void Disable()
        {
            m_userInput.Controls.TurnBasedPlayer.Disable();
        }

        public void Update()
        {
            if (!m_playerTurnIsActive)
                return;

            if (m_currentRoom == null)
                throw new NullReferenceException("m_currentRoom is not set");

            Vector2 currentMousePosition = m_userInput.Controls.TurnBasedPlayer.MousePosition.ReadValue<Vector2>();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(currentMousePosition);
            m_currentRoom.TryDisplayCombatTile(worldPosition);
        }

        public void StartCombat(Room room)
        {
            m_currentRoom = room;

            // Move player at the center of the current standing tile
            Vector3 newPosition = m_currentRoom.Grid.WorldToTileCenterPosition(m_playerTransform.position);
            m_playerTransform.position = newPosition;
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
            m_currentRoom.TryHidePreviousCombatTile();

            foreach (IDisplayable ui in m_combatUI)
                ui.Hide();

            NextTurn?.Invoke();
        }
    }
}