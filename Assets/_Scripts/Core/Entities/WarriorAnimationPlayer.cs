using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class WarriorAnimationPlayer : AnimationPlayer
    {
        private IMeleeAttacker m_attacker;
        private IMovable m_movement;
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        private bool m_triggerAttackAnimation = false;

        public override void Initialize()
        {
            base.Initialize();
            m_attacker = GetComponent<IMeleeAttacker>();
            m_movement = GetComponent<IMovable>();
        }

        public override void Enable()
        {
            SetAnimationState(m_idleAnimationID);
            m_attacker.OnAttackStarted += TriggerAttackAnimation;
        }

        public override void Disable()
        {
            m_triggerAttackAnimation = false;
            m_attacker.OnAttackStarted -= TriggerAttackAnimation;
        }

        public override int GetAnimationState()
        {
            if (m_triggerAttackAnimation && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && CurrentAnimationState == m_attackAnimationID)
            {
                m_triggerAttackAnimation = false;
                m_attacker.OnAttackStarted += TriggerAttackAnimation;
            }

            if (m_triggerAttackAnimation)
                return m_attackAnimationID;

            if (m_movement.CurrentMovementDirection.magnitude > 0)
                return m_runAnimationID;

            return m_idleAnimationID;
        }

        private void TriggerAttackAnimation()
        {
            m_triggerAttackAnimation = true;
            m_attacker.OnAttackStarted -= TriggerAttackAnimation;
        }
    }
}