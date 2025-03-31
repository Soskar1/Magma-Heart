using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class WarriorAnimationPlayer : AnimationPlayer
    {
        private Warrior m_warrior;
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        private bool m_triggerAttackAnimation = false;

        public override void Awake()
        {
            base.Awake();
            m_warrior = GetComponent<Warrior>();
        }

        public void Start()
        {
            SetAnimationState(m_idleAnimationID);
            m_warrior.OnAttackStarted += TriggerAttackAnimation;
        }

        public override int GetAnimationState()
        {
            if (m_triggerAttackAnimation && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && CurrentAnimationState == m_attackAnimationID)
            {
                m_triggerAttackAnimation = false;
                m_warrior.OnAttackStarted += TriggerAttackAnimation;
            }

            if (m_triggerAttackAnimation)
                return m_attackAnimationID;

            if (m_warrior.CurrentMovementDirection.magnitude > 0)
                return m_runAnimationID;

            return m_idleAnimationID;
        }

        private void TriggerAttackAnimation()
        {
            m_triggerAttackAnimation = true;
            m_warrior.OnAttackStarted -= TriggerAttackAnimation;
        }
    }
}