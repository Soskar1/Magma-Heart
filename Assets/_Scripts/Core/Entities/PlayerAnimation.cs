using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public sealed class PlayerAnimation : AnimationPlayer
    {
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        public EventHandler OnAttackAnimationHitFrameTriggered;
        public EventHandler OnAttackAnimationEnded;

        public void OnEnable() => OnAnimationEnded += HandleOnAnimationEnded;
        public void OnDisable() => OnAnimationEnded -= HandleOnAnimationEnded;

        public void PlayIdleAnimation() => CurrentAnimationState = m_idleAnimationID;
        public void PlayRunAnimation() => CurrentAnimationState = m_runAnimationID;
        public void PlayAttackAnimation() => CurrentAnimationState = m_attackAnimationID;

        public void OnAttackAnimationHitFrame() => OnAttackAnimationHitFrameTriggered?.Invoke(this, EventArgs.Empty);

        private void HandleOnAnimationEnded(object obj, OnAnimationEndedEventArgs args)
        {
            if (args.AnimationStateHash == m_attackAnimationID)
                OnAttackAnimationEnded?.Invoke(obj, EventArgs.Empty);
        }
    }
}