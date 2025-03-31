using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class SkeletonAnimationPlayer : AnimationPlayer
    {
        private Skeleton m_skeleton;
        private readonly string m_takeDamageAnimationName = "TakeDamage";
        private readonly string m_deathAnimationName = "Death";
        private readonly string m_attackAnimationName = "Attack";

        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_walkAnimationID = Animator.StringToHash("Walk");
        private int m_attackAnimationID;
        private int m_takeDamageAnimationID;
        private int m_deathAnimationID;

        private bool m_triggerAttackAnimation = false;
        private bool m_triggerTakeDamageAnimation = false;
        private bool m_triggerDeathAnimation = false;

        public override void Awake()
        {
            base.Awake();
            m_skeleton = GetComponent<Skeleton>();
            m_takeDamageAnimationID = Animator.StringToHash(m_takeDamageAnimationName);
            m_deathAnimationID = Animator.StringToHash(m_deathAnimationName);
            m_attackAnimationID = Animator.StringToHash(m_attackAnimationName);
        }

        private void OnEnable()
        {
            m_skeleton.OnAttackStarted += TriggerAttackAnimation;
            m_skeleton.Health.OnTakeDamage += TriggerTakeDamageAnimation;
            m_skeleton.Health.OnDeath += TriggerDeathAnimation;
            SetAnimationState(m_idleAnimationID);
        }

        private void OnDisable()
        {
            m_skeleton.OnAttackStarted -= TriggerAttackAnimation;
            m_skeleton.Health.OnTakeDamage -= TriggerTakeDamageAnimation;
            m_skeleton.Health.OnDeath -= TriggerDeathAnimation;
        }

        public override int GetAnimationState()
        {
            if (m_triggerDeathAnimation && DoesCurrentAnimationEnded() && CurrentAnimationState == m_deathAnimationID)
                OnAnimationEnded?.Invoke(m_deathAnimationName);

            if (m_triggerDeathAnimation)
                return m_deathAnimationID;

            if (m_triggerTakeDamageAnimation && DoesCurrentAnimationEnded() && CurrentAnimationState == m_takeDamageAnimationID)
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

            if (m_triggerAttackAnimation && DoesCurrentAnimationEnded() && CurrentAnimationState == m_attackAnimationID)
            {
                m_triggerAttackAnimation = false;

                m_skeleton.OnAttackEnded?.Invoke();
                OnAnimationEnded?.Invoke(m_attackAnimationName);
                
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

        private void TriggerDeathAnimation()
        {
            m_triggerDeathAnimation = true;
            m_skeleton.Health.OnDeath -= TriggerDeathAnimation;
        }
    }
}