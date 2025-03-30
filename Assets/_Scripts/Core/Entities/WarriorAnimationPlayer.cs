using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class WarriorAnimationPlayer : AnimationPlayer
    {
        private readonly IEntity m_entityToAnimate;
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        private bool m_triggerAttackAnimation = false;

        public WarriorAnimationPlayer(in Animator animator, IEntity entity) : base(animator)
        {
            m_entityToAnimate = entity;

            SetAnimationState(m_idleAnimationID);
            m_entityToAnimate.OnAttack += TriggerAttackAnimation;
        }

        public override int GetAnimationState()
        {
            if (m_triggerAttackAnimation && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && CurrentAnimationState == m_attackAnimationID)
            {
                m_triggerAttackAnimation = false;
                m_entityToAnimate.OnAttack += TriggerAttackAnimation;
            }

            if (m_triggerAttackAnimation)
                return m_attackAnimationID;

            if (m_entityToAnimate.CurrentMovementDirection.magnitude > 0)
                return m_runAnimationID;

            return m_idleAnimationID;
        }

        private void TriggerAttackAnimation()
        {
            m_triggerAttackAnimation = true;
            m_entityToAnimate.OnAttack -= TriggerAttackAnimation;
        }
    }
}