using System;
using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        public Action<string> OnAnimationEnded;

        public Animator Animator { get; private set; }

        private int m_currentAnimationState;
        public int CurrentAnimationState
        {
            get => m_currentAnimationState;
            set => m_currentAnimationState = value;
        }

        public void Awake() => Animator = GetComponent<Animator>();

        public void PlayAnimations()
        {
            Animator.CrossFade(CurrentAnimationState, 0);
        }

        public bool DoesCurrentAnimationEnded() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;

        public void IncreaseAnimationSpeed(string animationParameter, float amount)
        {
            float currentAmount = Animator.GetFloat(animationParameter);
            Animator.SetFloat(animationParameter, currentAmount + amount);
        }
    }
}