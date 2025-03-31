using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Skeleton : Entity, IMovable, IAttacker
    {
        private RigidbodyMovement m_movement;
        private Facing m_facing;
        private SkeletonAnimationPlayer m_animationPlayer;

        private Action m_onAttackStarted;
        private Action m_onAttackEnded;

        public Action OnAttackStarted { get => m_onAttackStarted; set => m_onAttackStarted = value; }
        public Action OnAttackEnded { get => m_onAttackEnded; set => m_onAttackEnded = value; }
        public Vector2 CurrentMovementDirection { get; private set; }
        

        public override void Awake()
        {
            base.Awake();
            m_movement = GetComponent<RigidbodyMovement>();
            m_facing = GetComponent<Facing>();
            m_animationPlayer = GetComponent<SkeletonAnimationPlayer>();
        }

        public void Update()
        {
            m_facing.TryUpdateFacing(CurrentMovementDirection.x);
            m_animationPlayer.PlayAnimations();
        }

        public void FixedUpdate()
        {
            m_movement.Move(CurrentMovementDirection);
        }

        public void SetMovementDirection(Vector2 direction) => CurrentMovementDirection = direction;

        public void Attack() => m_onAttackStarted?.Invoke();
    }
}