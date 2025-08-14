using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedPlayerBehaviour : IPlayerBehaviour, ICombatController
    {
        private TurnBasedUserInput m_userInput;
        private RoomTile m_currentMouseTile;
        private Action m_onMouseClicked;

        private Room m_currentRoom;

        private Transform m_playerTransform;
        private Health m_health;
        private Energy m_energy;
        private EnergyHUD m_energyHUD;

        private MovementAction m_movementAction;
        private AttackAction m_attackAction;
        private IHittableTile m_currentMouseOverEntity;

        public Transform Transform => m_playerTransform;
        public Vector3Int CurrentTilePosition { get; set; }
        public EventHandler NextTurn { get; set; }
        public bool IsPlayableCharacter => true;

        public Health Health => m_health;

        private bool m_playerTurnIsActive;

        public TurnBasedPlayerBehaviour(Player player, TurnBasedUserInput userInput, EnergyHUD energyHUD)
        {
            m_playerTransform = player.transform;
            m_energy = player.Energy;
            m_health = player.Health;
            m_userInput = userInput;
            m_energyHUD = energyHUD;
            m_playerTurnIsActive = false;

            m_movementAction = new MovementAction(m_energy, this);
            m_attackAction = new AttackAction(m_energy, this);
        }

        public void Enable() => m_energy.OnEnergyChanged += m_energyHUD.DisplayEnergy;

        public void Disable() => m_energy.OnEnergyChanged -= m_energyHUD.DisplayEnergy;

        public void Update()
        {
            if (!m_playerTurnIsActive)
                return;

            m_userInput.MouseControl.UpdateMousePosition();
        }

        public void Hit(float damage) => m_health.TakeDamage(damage);

        public void StartCombat(Room room)
        {
            m_currentRoom = room;
            m_movementAction.SetCurrentRoom(m_currentRoom);

            // Move player at the center of the current standing tile
            RoomTile roomTile = m_currentRoom.GetRoomTile(m_playerTransform.position);
            m_movementAction.Move(roomTile);
        }

        public void StartTurn()
        {
            m_userInput.Enable();
            m_userInput.MouseControl.OnMouseChangedTile += HandleOnMouseChangeTile;
            m_userInput.MouseControl.OnMouseClicked += HandleOnMouseClicked;

            m_energy.Regenerate();

            Debug.Log("Player is doing a move");
            m_playerTurnIsActive = true;
        }

        public void EndTurn()
        {
            m_userInput.Disable();
            m_userInput.MouseControl.OnMouseChangedTile -= HandleOnMouseChangeTile;
            m_userInput.MouseControl.OnMouseClicked -= HandleOnMouseClicked;

            Debug.Log("Player ended his move");
            m_playerTurnIsActive = false;

            if (m_currentMouseTile != null)
            {
                m_currentRoom.HideCombatTileAt(m_currentMouseTile);
                m_currentMouseTile = null;
            }

            if (m_currentMouseOverEntity != null)
            {
                m_currentMouseOverEntity = null;
            }

            NextTurn?.Invoke(this, EventArgs.Empty);
            m_movementAction.Reset();
        }

        private void HandleOnMouseChangeTile(object obj, OnMouseChangedTileEventArgs e)
        {
            if (m_currentMouseTile != null)
                m_currentRoom.HideCombatTileAt(m_currentMouseTile);

            RoomTile roomTile = m_currentRoom.GetRoomTile(e.TilePosition);

            if (m_currentRoom.EntityIsOnTile(roomTile, out IHittableTile entity))
            {
                m_currentMouseOverEntity = entity;
                if (entity != this && m_attackAction.CanAttack(entity))
                {
                    // TODO: Outline the entity that can be attacked or display some kind of visual feedback
                    m_onMouseClicked = ApplyAttackAction;
                    m_energyHUD.DisplayEnergyPrice(AttackAction.ENERGY_COST);
                    Debug.Log($"Player can attack entity at tile {roomTile.Position}. Energy cost: {AttackAction.ENERGY_COST}");
                }
                else
                {
                    m_onMouseClicked = null;
                    m_energyHUD.DisplayEnergyPrice(0);
                    Debug.LogWarning($"Player cannot attack entity at tile {roomTile.Position} because of insufficient energy or tile is not accessible.");
                }
            }
            else
            {
                DisplayCostForMovementAction(roomTile);
                m_onMouseClicked = ApplyMovementAction;
                m_currentMouseOverEntity = null;
            }

            m_currentMouseTile = roomTile;
        }

        private void HandleOnMouseClicked(object obj, EventArgs e)
        {
            m_onMouseClicked?.Invoke();
        }

        private void DisplayCostForMovementAction(RoomTile roomTile)
        {
            if (m_movementAction.CanMoveToTile(roomTile))
            {
                m_currentRoom.TryDisplayCombatTile(roomTile);
                m_energyHUD.DisplayEnergyPrice(m_movementAction.CurrentTheoreticalEnergyUsage);
            }
            else
            {
                // TODO: Display some kind of warning (tooltip) that player doesn't have enough energy to move
                Debug.LogWarning($"Player cannot move to tile {roomTile.Position} because of insufficient energy or tile is not accessible.");
            }
        }

        private void ApplyMovementAction()
        {
            if (m_currentMouseTile == null)
                Debug.LogWarning("Player clicked on tile, but current mouse tile is not set. This should never happen!");

            Debug.Log($"Player clicked on tile {m_currentMouseTile}");

            m_movementAction.MoveWithEnergyCost(m_currentMouseTile);
        }

        private void ApplyAttackAction()
        {
            if (m_currentMouseOverEntity == null)
            {
                Debug.LogWarning("Player clicked on entity, but current mouse over entity is not set. This should never happen!");
                return;
            }

            m_attackAction.AttackWithEnergyCost(m_currentMouseOverEntity);

            if (m_currentMouseOverEntity != null && m_attackAction.CanAttack(m_currentMouseOverEntity))
                m_energyHUD.DisplayEnergyPrice(AttackAction.ENERGY_COST);
        }
    }
}