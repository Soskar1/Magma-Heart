using System;
using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimationPlayer : MonoBehaviour
    {
        public Action<string> OnAnimationEnded;
        private Animator m_animator;
        private int m_currentAnimationState;

        public Animator Animator => m_animator;
        public int CurrentAnimationState => m_currentAnimationState;

        public virtual void Awake() => m_animator = GetComponent<Animator>();
        public abstract void Enable();
        public abstract void Disable();

        public void SetAnimationState(int animationState) => m_currentAnimationState = animationState;

        public void PlayAnimations()
        {
            int stateToPlay = GetAnimationState();

            if (stateToPlay == m_currentAnimationState)
                return;

            m_animator.CrossFade(stateToPlay, 0);
            m_currentAnimationState = stateToPlay;
        }

        public abstract int GetAnimationState();

        public bool DoesCurrentAnimationEnded() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;

        public void IncreaseAnimationSpeed(string animationParameter, float amount)
        {
            float currentAmount = m_animator.GetFloat(animationParameter);
            m_animator.SetFloat(animationParameter, currentAmount + amount);
        }
    }
}