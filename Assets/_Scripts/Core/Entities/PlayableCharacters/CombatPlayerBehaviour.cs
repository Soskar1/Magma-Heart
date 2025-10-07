using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.UI;
using MagmaHeart.Navigation;
using System;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class CombatPlayerBehaviour : IPlayerBehaviour, ICombatController
    {
        private CombatUserInput m_userInput;
        private RoomTile m_currentMouseTile;
        private IHittableTile m_currentMouseOverEntity;

        private Health m_health;
        public Health Health => m_health;

        private readonly Energy m_energy;
        private readonly EnergyHUD m_energyHUD;
        private readonly CombatUI m_combatUI;

        private PlayerAnimation m_animation;
        private Facing m_facing;

        private Room m_currentRoom;
        private ICombatAction m_currentAction;
        private MovementAction m_movementAction;
        private AttackAction m_attackAction;

        private TurnBasedMovement m_movement;
        private PathGizmosRenderer m_aStarPathRenderer;
        public EventHandler<OnMovementEventArgs> OnMoved { get; set; }

        private Transform m_playerTransform;
        public Transform Transform => m_playerTransform;
        public Vector3Int CurrentTilePosition => m_currentRoom.Grid.WorldToTilePosition(m_playerTransform.position);
        public EventHandler NextTurn { get; set; }
        public bool IsPlayableCharacter => true;

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

        public CombatPlayerBehaviour(Player player, CombatUserInput userInput, GameUI gameUI)
        {
            m_playerTransform = player.transform;
            m_energy = player.Energy;
            m_health = player.Health;
            m_userInput = userInput;
            m_energyHUD = gameUI.EnergyHUD;
            m_combatUI = gameUI.CombatUI;
            m_playerTurnIsActive = false;

            m_facing = player.GetComponent<Facing>();
            m_animation = player.GetComponent<PlayerAnimation>();
            m_movement = player.GetComponent<TurnBasedMovement>();
            m_movementAction = new MovementAction(m_movement, m_energy, this);
            m_attackAction = new AttackAction(m_energy, this);

            m_aStarPathRenderer = player.GetComponent<PathGizmosRenderer>(); // For debug purposes
        }

        public void Enable()
        {
            m_userInput.Enable();
            m_movementAction.Enable();

            m_energy.OnEnergyChanged += m_energyHUD.DisplayEnergy;

            m_movement.OnMovementStarted += HandleOnMovementStarted;
            m_movement.OnMovementStarted += m_combatUI.HandleOnMovementStarted;
            m_movement.OnMovementEnded += HandleOnMovementEnded;
            m_movement.OnMovementEnded += m_combatUI.HandleOnMovementEnded;

            m_attackAction.OnAttackTriggered += HandleOnAttackTriggered;
            m_attackAction.OnAttackTriggered += m_combatUI.HandleOnAttackTriggered;

            m_animation.OnAttackAnimationHitFrameTriggered += HandleOnAttackAnimationHitFrame;
            m_animation.OnAttackAnimationEnded += HandleOnAttackAnimationEnded;
            m_animation.OnAttackAnimationEnded += m_combatUI.HandleOnAttackAnimationEnded;

            m_animation.PlayIdleAnimation();
        }

        public void Disable()
        {
            m_userInput.Disable();
            m_movementAction.Disable();

            m_energy.OnEnergyChanged -= m_energyHUD.DisplayEnergy;

            m_movement.OnMovementStarted -= HandleOnMovementStarted;
            m_movement.OnMovementStarted -= m_combatUI.HandleOnMovementStarted;
            m_movement.OnMovementEnded -= HandleOnMovementEnded;
            m_movement.OnMovementEnded -= m_combatUI.HandleOnMovementEnded;

            m_attackAction.OnAttackTriggered -= HandleOnAttackTriggered;
            m_attackAction.OnAttackTriggered -= m_combatUI.HandleOnAttackTriggered;

            m_animation.OnAttackAnimationHitFrameTriggered -= HandleOnAttackAnimationHitFrame;
            m_animation.OnAttackAnimationEnded -= HandleOnAttackAnimationEnded;
            m_animation.OnAttackAnimationEnded -= m_combatUI.HandleOnAttackAnimationEnded;
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
            m_userInput.MouseControl.OnMouseChangedTile += HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked += HandleOnMouseClicked;

            m_energy.Regenerate();

            Debug.Log("Player is doing a move");
            m_playerTurnIsActive = true;
        }

        public void EndTurn()
        {
            m_userInput.MouseControl.OnMouseChangedTile -= HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked -= HandleOnMouseClicked;

            m_playerTurnIsActive = false;

            if (m_currentMouseTile != null)
            {
                m_currentRoom.HideCombatTileAt(m_currentMouseTile);
                m_currentMouseTile = null;
            }

            if (m_currentMouseOverEntity != null)
                m_currentMouseOverEntity = null;

            NextTurn?.Invoke(this, EventArgs.Empty);
            m_movementAction.Reset();
        }

        private void HandleOnMouseChangedTile(object obj, OnMouseChangedTileEventArgs e)
        {
            if (!CanExecuteAction)
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
            if (m_currentAction == null || !CanExecuteAction)
                return;

            m_currentAction.Execute();
            m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
        }
        
        private void HandleOnMovementStarted(object obj, OnMovementEventArgs e)
        {
            CanExecuteAction = false;
            m_animation.PlayRunAnimation();

            m_facing.TryUpdateFacing((e.To - e.From).x);
        }

        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            CanExecuteAction = true;
            m_animation.PlayIdleAnimation();
        }

        private void HandleOnAttackTriggered(object obj, OnAttackEventArgs e)
        {
            CanExecuteAction = false;

            m_facing.TryUpdateFacing(e.EntityPosition.x - Transform.position.x);
            m_animation.PlayAttackAnimation();
        }

        private void HandleOnAttackAnimationHitFrame(object obj, EventArgs e) => m_attackAction.Hit();

        private void HandleOnAttackAnimationEnded(object obj, EventArgs e)
        {
            CanExecuteAction = true;
            m_animation.PlayIdleAnimation();
        }
    }
}