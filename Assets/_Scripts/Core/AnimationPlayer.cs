using System;
using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        public EventHandler<OnAnimationEndedEventArgs> OnAnimationEnded;

        public Animator Animator { get; private set; }

        private int m_currentAnimationState;
        public int CurrentAnimationState
        {
            get => m_currentAnimationState;
            set
            {
                m_currentAnimationState = value;
                Animator.CrossFade(CurrentAnimationState, 0);
            }
        }

        public void Awake() => Animator = GetComponent<Animator>();

        public void PlayAnimations()
        {
            if (DoesCurrentAnimationEnded())
            {
                Animator.CrossFade(CurrentAnimationState, 0, 0, 0);

                OnAnimationEndedEventArgs args = new OnAnimationEndedEventArgs(CurrentAnimationState);
                OnAnimationEnded?.Invoke(this, args);
            }
        }

        public bool DoesCurrentAnimationEnded() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;

        public void IncreaseAnimationSpeed(string animationParameter, float amount)
        {
            float currentAmount = Animator.GetFloat(animationParameter);
            Animator.SetFloat(animationParameter, currentAmount + amount);
        }
    }
}