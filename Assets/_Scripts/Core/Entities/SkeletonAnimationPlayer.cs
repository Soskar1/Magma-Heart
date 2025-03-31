using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class SkeletonAnimationPlayer : AnimationPlayer
    {
        private Skeleton m_skeleton;
        private readonly string m_takeDamageAnimationName = "TakeDamage";

        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_walkAnimationID = Animator.StringToHash("Walk");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");
        private int m_takeDamageAnimationID;

        private bool m_triggerAttackAnimation = false;
        private bool m_triggerTakeDamageAnimation = false;

        public override void Awake()
        {
            base.Awake();
            m_skeleton = GetComponent<Skeleton>();
            m_takeDamageAnimationID = Animator.StringToHash(m_takeDamageAnimationName);
        }

        private void OnEnable()
        {
            m_skeleton.OnAttackStarted += TriggerAttackAnimation;
            m_skeleton.Health.OnTakeDamage += TriggerTakeDamageAnimation;
            SetAnimationState(m_idleAnimationID);
        }

        private void OnDisable()
        {
            m_skeleton.OnAttackStarted -= TriggerAttackAnimation;
            m_skeleton.Health.OnTakeDamage -= TriggerTakeDamageAnimation;
        }

        public override int GetAnimationState()
        {
            if (m_triggerTakeDamageAnimation && GetAnimationPlayTime() > 1 && CurrentAnimationState == m_takeDamageAnimationID)
            {
                m_triggerTakeDamageAnimation = false;
                OnAnimationEnded?.Invoke(m_takeDamageAnimationName);
                m_skeleton.Health.OnTakeDamage += TriggerTakeDamageAnimation;

                if (m_triggerAttackAnimation)
                {
                    m_triggerAttackAnimation = false;
                    m_skeleton.OnAttackStarted += TriggerAttackAnimation;
                }
            }

            if (m_triggerTakeDamageAnimation)
                return m_takeDamageAnimationID;

            if (m_triggerAttackAnimation && GetAnimationPlayTime() > 1 && CurrentAnimationState == m_attackAnimationID)
            {
                m_skeleton.OnAttackEnded?.Invoke();
                m_triggerAttackAnimation = false;
                m_skeleton.OnAttackStarted += TriggerAttackAnimation;
            }

            if (m_triggerAttackAnimation)
                return m_attackAnimationID;

            if (m_skeleton.CurrentMovementDirection.magnitude > 0)
                return m_walkAnimationID;

            return m_idleAnimationID;
        }

        private void TriggerAttackAnimation()
        {
            m_triggerAttackAnimation = true;
            m_skeleton.OnAttackStarted -= TriggerAttackAnimation;
        }

        private void TriggerTakeDamageAnimation()
        {
            m_triggerTakeDamageAnimation = true;
            m_skeleton.Health.OnTakeDamage -= TriggerTakeDamageAnimation;
        }
    }
}