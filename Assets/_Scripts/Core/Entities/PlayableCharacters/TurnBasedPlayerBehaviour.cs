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

        private Vector3Int? m_currentMouseTile;

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

            m_movementAction = new MovementAction(player.ControllingEntity);
        }

        public void Enable()
        {
            m_energy.OnEnergyChanged += m_energyHUD.DisplayEnergy;
        }

        public void Disable()
        {
            m_energy.OnEnergyChanged -= m_energyHUD.DisplayEnergy;
        }

        public void Update()
        {
            if (!m_playerTurnIsActive)
                return;

            m_userInput.MouseControl.UpdateMousePosition();
        }

        private void DisplayCostForMovementAction(Vector3Int mouseRoomTilePosition)
        {
            if (m_currentMouseTile.HasValue)
                m_currentRoom.HideCombatTileAt(m_currentMouseTile.Value);

            (int energyCost, _) = m_movementAction.CalculateEnergyUsage(mouseRoomTilePosition);

            if (m_energy.HasEnough(energyCost))
            {
                m_currentRoom.TryDisplayCombatTile(mouseRoomTilePosition);
                m_energyHUD.DisplayEnergyPrice(energyCost);
            }
            else
            {
                // TODO: Display some kind of warning (tooltip) that player doesn't have enough energy to move
            }

            m_currentMouseTile = mouseRoomTilePosition;
        }

        private void ApplyMovementAction()
        {
            if (!m_currentMouseTile.HasValue)
                Debug.LogWarning("Player clicked on tile, but current mouse tile is not set. This should never happen!");

            Debug.Log($"Player clicked on tile {m_currentMouseTile}");

            m_movementAction.TryMove(m_currentMouseTile.Value);
        }

        public void StartCombat(Room room)
        {
            m_currentRoom = room;
            m_movementAction.SetCurrentRoom(m_currentRoom);

            // Move player at the center of the current standing tile
            m_playersCurrentTile = m_currentRoom.Grid.WorldToTilePosition(m_playerTransform.position);
            m_playerTransform.position = m_currentRoom.Grid.ToTileCenter(m_playersCurrentTile);
        }

        public void StartTurn()
        {
            m_userInput.Enable();
            m_userInput.MouseControl.OnMouseChangedTile += DisplayCostForMovementAction;
            m_userInput.MouseControl.OnMouseClicked += ApplyMovementAction;

            m_energy.Regenerate();

            Debug.Log("Player is doing a move");
            m_playerTurnIsActive = true;
        }

        public void EndTurn()
        {
            m_userInput.Disable();
            m_userInput.MouseControl.OnMouseChangedTile -= DisplayCostForMovementAction;
            m_userInput.MouseControl.OnMouseClicked -= ApplyMovementAction;

            Debug.Log("Player ended his move");
            m_playerTurnIsActive = false;

            if (m_currentMouseTile.HasValue)
            {
                m_currentRoom.HideCombatTileAt(m_currentMouseTile.Value);
                m_currentMouseTile = null;
            }

            NextTurn?.Invoke();
            m_movementAction.Reset();
        }
    }
}