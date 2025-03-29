using System;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class AnimationPlayer
    {
        private Animator m_animator;
        private int m_currentAnimationState;
        private Func<int> m_animationStateGetter;

        public AnimationPlayer(in Animator animator, in int startAnimationState, in Func<int> animationStateGetter)
        {
            m_animator = animator;
            m_currentAnimationState = startAnimationState;
            m_animationStateGetter = animationStateGetter;
        }

        public void PlayAnimations()
        {
            int stateToPlay = m_animationStateGetter();

            if (stateToPlay == m_currentAnimationState)
                return;

            m_animator.CrossFade(stateToPlay, 0);
            m_currentAnimationState = stateToPlay;
        }
    }
}