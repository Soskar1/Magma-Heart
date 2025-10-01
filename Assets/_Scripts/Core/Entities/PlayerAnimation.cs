using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public sealed class PlayerAnimation : AnimationPlayer
    {
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        public void PlayIdleAnimation() => CurrentAnimationState = m_idleAnimationID;

        public void PlayRunAnimation() => CurrentAnimationState = m_runAnimationID;
    }
}