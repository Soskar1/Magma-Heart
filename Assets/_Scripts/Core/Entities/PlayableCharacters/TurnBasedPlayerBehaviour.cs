using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using MagmaHeart.Navigation;
using System;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class TurnBasedPlayerBehaviour : IPlayerBehaviour, ICombatController
    {
        private TurnBasedUserInput m_userInput;
        private RoomTile m_currentMouseTile;

        private Room m_currentRoom;

        private Transform m_playerTransform;
        private Health m_health;
        private Energy m_energy;
        private EnergyHUD m_energyHUD;

        private ICombatAction m_currentAction;

        private MovementAction m_movementAction;
        private TurnBasedMovement m_movement;
        private PathGizmosRenderer m_aStarPathRenderer;
        public EventHandler<OnMovedEventArgs> OnMoved { get; set; }

        private AttackAction m_attackAction;
        private IHittableTile m_currentMouseOverEntity;

        public Transform Transform => m_playerTransform;
        public Vector3Int CurrentTilePosition => m_currentRoom.Grid.WorldToTilePosition(m_playerTransform.position);
        public EventHandler NextTurn { get; set; }
        public bool IsPlayableCharacter => true;

        public Health Health => m_health;

        private bool m_playerTurnIsActive;
        private bool m_canExecuteActions;

        private bool CanExecuteAction
        {
            get => m_canExecuteActions;
            set
            {
                m_canExecuteActions = value;
                if (m_canExecuteActions)
                    m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
            }
        }

        public TurnBasedPlayerBehaviour(Player player, TurnBasedUserInput userInput, EnergyHUD energyHUD)
        {
            m_playerTransform = player.transform;
            m_energy = player.Energy;
            m_health = player.Health;
            m_userInput = userInput;
            m_energyHUD = energyHUD;
            m_playerTurnIsActive = false;

            m_movement = player.GetComponent<TurnBasedMovement>();
            m_movementAction = new MovementAction(m_movement, m_energy, this);
            m_attackAction = new AttackAction(m_energy, this);

            m_aStarPathRenderer = player.GetComponent<PathGizmosRenderer>(); // For debug purposes
        }

        public void Enable()
        {
            m_energy.OnEnergyChanged += m_energyHUD.DisplayEnergy;
            m_movement.OnMovementStarted += DisableActionExecution;
            m_movement.OnMovementEnded += EnableActionExecution;
        }

        public void Disable()
        {
            m_energy.OnEnergyChanged -= m_energyHUD.DisplayEnergy;
            m_movement.OnMovementStarted -= DisableActionExecution;
            m_movement.OnMovementEnded -= EnableActionExecution;
        }

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
            m_movementAction.MoveWithoutEnergyUsage(roomTile);
        }

        public void StartTurn()
        {
            m_userInput.Enable();
            m_userInput.MouseControl.OnMouseChangedTile += HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked += HandleOnMouseClicked;

            m_energy.Regenerate();

            Debug.Log("Player is doing a move");
            m_playerTurnIsActive = true;
        }

        public void EndTurn()
        {
            m_userInput.Disable();
            m_userInput.MouseControl.OnMouseChangedTile -= HandleOnMouseChangedTile;
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

        private void HandleOnMouseChangedTile(object obj, OnMouseChangedTileEventArgs e)
        {
            if (!m_canExecuteActions)
                return;

            if (m_currentMouseTile != null)
                m_currentRoom.HideCombatTileAt(m_currentMouseTile);

            RoomTile roomTile = m_currentRoom.GetRoomTile(e.TilePosition);

            if (m_currentRoom.EntityIsOnTile(roomTile, out IHittableTile entity))
            {
                m_currentMouseOverEntity = entity;
                if (entity != this && m_attackAction.CanAttack(entity))
                {
                    // TODO: Outline the entity that can be attacked or display some kind of visual feedback
                    m_attackAction.EntityToHit = entity;
                    m_currentAction = m_attackAction;
                    m_energyHUD.DisplayEnergyPrice(AttackAction.ENERGY_COST);
                }
                else
                {
                    m_currentAction = null;
                    m_energyHUD.DisplayEnergyPrice(0);
                }
            }
            else if (m_movementAction.CanMoveToTile(roomTile))
            {
                m_currentMouseOverEntity = null;
                m_movementAction.TileToMove = roomTile;
                m_currentAction = m_movementAction;

                m_currentRoom.TryDisplayCombatTile(roomTile);
                m_energyHUD.DisplayEnergyPrice(m_movementAction.CurrentTheoreticalEnergyUsage);
                m_aStarPathRenderer.CurrentPath = m_movementAction.CurrentPath.Select(tile => tile.TileCenter).ToList();
            }
            else
            {
                m_currentAction = null;

                // TODO: Display some kind of warning (tooltip) that player doesn't have enough energy to move
                Debug.LogWarning($"Player cannot move to tile {roomTile.Position} because of insufficient energy or tile is not accessible.");
            }

            m_currentMouseTile = roomTile;
        }

        private void HandleOnMouseClicked(object obj, EventArgs e)
        {
            if (m_currentAction == null || !m_canExecuteActions)
                return;

            m_currentAction.Execute();
            m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
        }

        private void EnableActionExecution(object obj, EventArgs e) => CanExecuteAction = true;
        private void DisableActionExecution(object obj, EventArgs e) => CanExecuteAction = false;
    }
}