using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.UI;
using MagmaHeart.AI.Pathfinding;
using System;
using System.Linq;
using UnityEngine;
using MagmaHeart.Core.Entities.CombatSystem;
using MagmaHeart.AI;
using MagmaHeart.AI.Actions;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class CombatPlayerBehaviour : IPlayerBehaviour, IBattleStartedListener
    {
        private CombatUserInput m_userInput;
        private RoomTile m_currentMouseTile;

        private AIUnit m_currentMouseOverEntity;
        private EntityModel m_currentTargetedEntity;

        private readonly Player m_player;
        private readonly Energy m_energy;
        private readonly Transform m_transform;
        private readonly EnergyHUD m_energyHUD;
        private readonly CombatUI m_combatUI;
        private readonly CombatController m_combatController;

        private EntityAnimation m_animation;
        private Facing m_facing;

        private Room m_currentRoom;

        private MagmaHeart.AI.Actions.Action m_currentAction;
        private ActionArgs m_currentActionArgs;
        private MovementAction m_movementAction;
        private AttackAction m_attackAction;

        private TurnBasedMovement m_movement;
        private PathGizmosRenderer m_aStarPathRenderer;

        private bool m_canExecuteActions;

        public bool CanExecuteAction
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
            m_player = player;
            m_energy = m_player.Energy;
            m_transform = player.transform;
            m_combatController = player.CombatController;
            m_userInput = userInput;
            m_energyHUD = gameUI.EnergyHUD;
            m_combatUI = gameUI.CombatUI;

            m_facing = player.GetComponent<Facing>();
            m_animation = player.GetComponent<EntityAnimation>();
            m_movement = player.GetComponent<TurnBasedMovement>();

            m_movementAction = m_player.Model.PossibleActions.Get<MovementAction>();
            m_attackAction = m_player.Model.PossibleActions.Get<AttackAction>();

            m_aStarPathRenderer = player.GetComponent<PathGizmosRenderer>(); // For debug purposes
        }

        public void Enable()
        {
            m_userInput.Enable();

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

            m_combatController.OnTurnStarted += HandleOnTurnStarted;
            m_combatController.OnTurnEnded += HandleOnTurnEnded;

            m_animation.PlayIdleAnimation();
        }

        public void Disable()
        {
            m_userInput.Disable();

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

            m_combatController.OnTurnStarted -= HandleOnTurnStarted;
            m_combatController.OnTurnEnded -= HandleOnTurnEnded;
        }

        public void Update()
        {
            if (!CanExecuteAction)
                return;

            m_userInput.MouseControl.UpdateMousePosition();
        }

        public void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args)
        {
            m_currentRoom = args.Room;
            m_movementAction.SetCurrentRoom(m_currentRoom);

            // Move player at the center of the current standing tile
            RoomTile roomTile = m_currentRoom.GetRoomTile(m_transform.position);
            m_movementAction.MoveWithoutEnergyUsage(roomTile);
        }

        private void HandleOnTurnStarted(object obj, EventArgs args)
        {
            m_userInput.MouseControl.OnMouseChangedTile += HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked += HandleOnMouseClicked;

            m_energy.Regenerate();

            Debug.Log("Player is doing a move");
        }

        private void HandleOnTurnEnded(object obj, EventArgs args)
        {
            m_userInput.MouseControl.OnMouseChangedTile -= HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked -= HandleOnMouseClicked;

            if (m_currentMouseTile != null)
            {
                m_currentRoom.HideCombatTileAt(m_currentMouseTile);
                m_currentMouseTile = null;
            }

            if (m_currentMouseOverEntity != null)
                m_currentMouseOverEntity = null;

            m_movementAction.Reset();
        }

        private void HandleOnMouseChangedTile(object obj, OnMouseChangedTileEventArgs e)
        {
            if (!CanExecuteAction)
                return;

            if (m_currentMouseTile != null)
                m_currentRoom.HideCombatTileAt(m_currentMouseTile);

            RoomTile roomTile = m_currentRoom.GetRoomTile(e.TilePosition);

            if (m_currentRoom.EntityIsOnTile(roomTile, out EntityModel entity))
            {
                m_currentMouseOverEntity = entity;
                if (!entity.IsPlayer && m_attackAction.CanAttack(entity))
                {
                    // TODO: Outline the entity that can be attacked or display some kind of visual feedback
                    m_currentAction = m_attackAction;
                    m_currentActionArgs = new AttackActionArgs(entity);

                    m_energyHUD.DisplayEnergyPrice(AttackAction.ENERGY_COST);
                }
                else
                {
                    m_currentAction = null;
                    m_energyHUD.DisplayEnergyPrice(0);
                }
            }
            else if (m_movementAction.CanExecute(roomTile))
            {
                m_currentMouseOverEntity = null;

                m_currentAction = m_movementAction;
                m_currentActionArgs = new MovementActionArgs(roomTile);

                m_currentRoom.TryDisplayCombatTile(roomTile);
                m_energyHUD.DisplayEnergyPrice(m_movementAction.CurrentTheoreticalEnergyUsage);
                // m_aStarPathRenderer.CurrentPath = m_movementAction.CurrentPath.Select(tile => tile.TileCenter).ToList();
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

            m_currentAction.Execute(m_currentActionArgs);
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

            m_currentTargetedEntity = e.Target;
            m_facing.TryUpdateFacing(m_currentTargetedEntity.GetCurrentTilePosition().x - m_transform.position.x);
            m_animation.PlayAttackAnimation();
        }

        private void HandleOnAttackAnimationHitFrame(object obj, EventArgs e) => m_attackAction.Hit(m_currentTargetedEntity);

        private void HandleOnAttackAnimationEnded(object obj, EventArgs e)
        {
            CanExecuteAction = true;
            m_animation.PlayIdleAnimation();
            m_currentTargetedEntity = null;
        }
    }
}