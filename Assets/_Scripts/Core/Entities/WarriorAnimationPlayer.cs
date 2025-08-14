using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class WarriorAnimationPlayer : AnimationPlayer
    {
        private IMovable m_movement;
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        private bool m_triggerAttackAnimation = false;

        public override void Awake()
        {
            base.Awake();
            m_movement = GetComponent<IMovable>();
        }

        public override void Enable()
        {
            SetAnimationState(m_idleAnimationID);
        }

        public override void Disable()
        {
            m_triggerAttackAnimation = false;
        }

        public override int GetAnimationState()
        {
            if (m_triggerAttackAnimation)
                return m_attackAnimationID;

            if (m_movement.CurrentMovementDirection.magnitude > 0)
                return m_runAnimationID;

            return m_idleAnimationID;
        }
    }
}