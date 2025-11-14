using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public sealed class EntityAnimation : AnimationPlayer
    {
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        public event EventHandler OnAttackAnimationHitFrameTriggered;
        public event EventHandler OnAttackAnimationEnded;

        public void PlayIdleAnimation() => CurrentAnimationState = m_idleAnimationID;
        public void PlayRunAnimation() => CurrentAnimationState = m_runAnimationID;
        public void PlayAttackAnimation() => CurrentAnimationState = m_attackAnimationID;

        public void OnAttackAnimationHitFrame() => OnAttackAnimationHitFrameTriggered?.Invoke(this, EventArgs.Empty);

        public override void FireOnAnimationEndedEvent()
        {
            if (CurrentAnimationState == m_attackAnimationID)
                OnAttackAnimationEnded?.Invoke(this, EventArgs.Empty);
        }  
    }
}