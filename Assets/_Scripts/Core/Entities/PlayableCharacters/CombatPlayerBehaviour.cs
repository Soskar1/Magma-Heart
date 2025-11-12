using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.UI;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class CombatPlayerBehaviour : IPlayerBehaviour
    {
        private readonly CombatUserInput m_userInput;
        
        private readonly Player m_player;
        private readonly PlayerCombatController m_combatController;
        private readonly Transform m_transform;
        
        private readonly Facing m_facing;
        private readonly EntityAnimation m_animation;

        private readonly CombatUI m_combatUI;

        private readonly AttackAction m_attackAction;
        private readonly TurnBasedMovement m_movement;
        
        private EntityModel m_currentTargetedEntity;

        public CombatPlayerBehaviour(Player player, CombatUserInput userInput, CombatUI combatUI)
        {
            m_player = player;
            m_transform = player.transform;
            m_combatController = (PlayerCombatController)player.CombatController;
            m_userInput = userInput;
            m_combatUI = combatUI;

            m_facing = player.GetComponent<Facing>();
            m_animation = player.GetComponent<EntityAnimation>();
            m_movement = player.GetComponent<TurnBasedMovement>();

            m_attackAction = m_player.Model.PossibleActions.Get<AttackAction>();
        }

        public void Enable()
        {
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
            if (!m_combatController.CanExecuteAction)
                return;

            m_userInput.MouseControl.UpdateMousePosition();
        }
        
        private void HandleOnMovementStarted(object obj, OnMovementEventArgs e)
        {
            m_combatController.CanExecuteAction = false;
            m_animation.PlayRunAnimation();

            m_facing.TryUpdateFacing((e.To - e.From).x);
        }

        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            m_combatController.CanExecuteAction = true;
            m_animation.PlayIdleAnimation();
        }

        private void HandleOnAttackTriggered(object obj, OnAttackEventArgs e)
        {
            m_combatController.CanExecuteAction = false;

            m_currentTargetedEntity = e.Target;
            m_facing.TryUpdateFacing(m_currentTargetedEntity.GetCurrentTilePosition().x - m_transform.position.x);
            m_animation.PlayAttackAnimation();
        }

        private void HandleOnAttackAnimationHitFrame(object obj, EventArgs e) => m_attackAction.Hit(m_currentTargetedEntity);

        private void HandleOnAttackAnimationEnded(object obj, EventArgs e)
        {
            m_combatController.CanExecuteAction = true;
            m_animation.PlayIdleAnimation();
            m_currentTargetedEntity = null;
        }
    }
}