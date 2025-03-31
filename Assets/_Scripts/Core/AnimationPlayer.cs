using UnityEngine;

namespace MagmaHeart.Core
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimationPlayer : MonoBehaviour
    {
        private Animator m_animator;
        private int m_currentAnimationState;

        public Animator Animator => m_animator;
        public int CurrentAnimationState => m_currentAnimationState;

        public virtual void Awake() => m_animator = GetComponent<Animator>();

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
    }
}