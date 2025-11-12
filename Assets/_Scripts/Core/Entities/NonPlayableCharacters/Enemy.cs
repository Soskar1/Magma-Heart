using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.CombatSystem;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : Entity
    {
        private EntityAnimation m_animation;

        private MovementAction m_movementAction;
        private AttackAction m_attackAction;

        private TurnBasedMovement m_movement;
        private Facing m_facing;

        private EntityModel m_currentTargetedEntity;

        public void Initialize(DungeonGrid grid)
        {
            base.Initialize(grid, false);

            m_animation = GetComponent<EntityAnimation>();
            m_movement = GetComponent<TurnBasedMovement>();
            m_facing = GetComponent<Facing>();

            m_movementAction = Model.PossibleActions.Get<MovementAction>();
            m_attackAction = Model.PossibleActions.Get<AttackAction>();

            m_movement.OnMovementStarted += HandleOnMovementStarted;
            m_movement.OnMovementEnded += HandleOnMovementEnded;

            m_attackAction.OnAttackTriggered += HandleOnAttackTriggered;

            m_animation.OnAttackAnimationHitFrameTriggered += HandleOnAttackAnimationHitFrame;
            m_animation.OnAttackAnimationEnded += HandleOnAttackAnimationEnded;
        }

        private void OnDisable()
        {
            m_movement.OnMovementStarted -= HandleOnMovementStarted;
            m_movement.OnMovementEnded -= HandleOnMovementEnded;

            m_attackAction.OnAttackTriggered -= HandleOnAttackTriggered;

            m_animation.OnAttackAnimationHitFrameTriggered -= HandleOnAttackAnimationHitFrame;
            m_animation.OnAttackAnimationEnded -= HandleOnAttackAnimationEnded;
        }

        // TODO: Try to think more about this. Maybe we can have a better way to set the current room for enemies
        public void StartCombat(Room room) => m_movementAction.SetCurrentRoom(room);

        private void HandleOnMovementStarted(object obj, OnMovementEventArgs e)
        {
            m_animation.PlayRunAnimation();

            m_facing.TryUpdateFacing((e.To - e.From).x);
        }

        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            m_animation.PlayIdleAnimation();

            // TODO: Consider enemy AI for more complex behavior. Need to add ActionEndedEvent to properly handle this
            Debug.Log($"{gameObject.name} {transform.position} is ending it's move after the movement action");
            CombatController.EndTurn();
        }

        private void HandleOnAttackTriggered(object obj, OnAttackEventArgs e)
        {
            m_currentTargetedEntity = e.Target;
            m_facing.TryUpdateFacing(m_currentTargetedEntity.GetCurrentTilePosition().x - transform.position.x);
            m_animation.PlayAttackAnimation();
        }

        private void HandleOnAttackAnimationHitFrame(object obj, EventArgs e) => m_attackAction.Hit(m_currentTargetedEntity);

        private void HandleOnAttackAnimationEnded(object obj, EventArgs e)
        {
            m_animation.PlayIdleAnimation();
            m_currentTargetedEntity = null;

            // TODO: Consider enemy AI for more complex behavior. Need to add ActionEndedEvent to properly handle this
            Debug.Log($"{gameObject.name} {transform.position} is ending it's move after the attack action");
            CombatController.EndTurn();
        }
    }
}