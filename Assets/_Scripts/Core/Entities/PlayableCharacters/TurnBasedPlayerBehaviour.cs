using System;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedPlayerBehaviour : IPlayerBehaviour, ICombatController
    {
        private TurnBasedUserInput m_userInput;
        private Room m_currentRoom;

        private Energy m_energy;
        private EnergyHUD m_energyHUD;

        private MovementAction m_movementAction;

        private Transform m_playerTransform;
        private Vector3Int m_playersCurrentTile;

        private Vector3Int? m_previousMousePosition;

        public Action NextTurn { get; set; }

        public bool IsPlayableCharacter => true;

        private bool m_playerTurnIsActive;

        public TurnBasedPlayerBehaviour(Player player, TurnBasedUserInput userInput, EnergyHUD energyHUD)
        {
            m_playerTransform = player.transform;
            m_energy = player.Energy;
            m_userInput = userInput;
            m_energyHUD = energyHUD;
            m_playerTurnIsActive = false;

            m_movementAction = new MovementAction();
        }

        public void Enable()
        {
            m_userInput.Enable();
            m_userInput.MouseControl.OnMouseChangedTile += HandleMovementAction;
        }

        public void Disable()
        {
            m_userInput.Disable();
            m_userInput.MouseControl.OnMouseChangedTile -= HandleMovementAction;
        }

        public void Update()
        {
            if (!m_playerTurnIsActive)
                return;

            m_userInput.MouseControl.UpdateMousePosition();
        }

        private void HandleMovementAction(Vector3Int mouseRoomTilePosition)
        {
            if (m_previousMousePosition.HasValue)
                m_currentRoom.HideCombatTileAt(m_previousMousePosition.Value);

            int distance = m_currentRoom.Grid.ManhattanDistance(m_playersCurrentTile, mouseRoomTilePosition);
            int energyAmountForMovementAction = m_movementAction.CalculateEnergyUsage(distance);
            Debug.Log($"Distance between {m_playersCurrentTile} and {mouseRoomTilePosition} is {distance} tiles. You need {energyAmountForMovementAction} energy to move there");

            if (m_energy.HasEnough(energyAmountForMovementAction))
            {
                m_currentRoom.TryDisplayCombatTile(mouseRoomTilePosition);
                m_energyHUD.DisplayEnergyPrice(energyAmountForMovementAction);
            }
            else
            {
                // TODO: Display some kind of warning (tooltip) that player doesn't have enough energy to move
                Debug.LogWarning("You don't have enough energy to move there");
            }

            m_previousMousePosition = mouseRoomTilePosition;
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
            m_energyHUD.DisplayAvailableEnergy();

            Debug.Log("Player is doing a move");
            m_playerTurnIsActive = true;
        }

        public void EndTurn()
        {
            Debug.Log("Player ended his move");
            m_playerTurnIsActive = false;
            m_previousMousePosition = null;
            if (m_userInput.MouseControl.CurrentMouseTile.HasValue)
            {
                m_currentRoom.HideCombatTileAt(m_userInput.MouseControl.CurrentMouseTile.Value);
                m_userInput.MouseControl.ClearMousePosition();
            }

            NextTurn?.Invoke();
        }
    }
}