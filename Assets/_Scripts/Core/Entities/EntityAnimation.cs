using System;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public sealed class EntityAnimation : AnimationPlayer
    {
        private readonly int m_idleAnimationID = Animator.StringToHash("Idle");
        private readonly int m_runAnimationID = Animator.StringToHash("Run");
        private readonly int m_attackAnimationID = Animator.StringToHash("Attack");

        public event EventHandler OnAttackAnimationEnded;

        private TaskCompletionSource<bool> m_animationTrigger;

        public void PlayIdleAnimation() => CurrentAnimationState = m_idleAnimationID;
        public void PlayRunAnimation() => CurrentAnimationState = m_runAnimationID;
        public void PlayAttackAnimation() => CurrentAnimationState = m_attackAnimationID;
        public Task PlayAttackAnimationAsync()
        {
            CurrentAnimationState = m_attackAnimationID;
            return GetAnimationTriggerTask();
        }

        public Task GetAnimationTriggerTask()
        {
            m_animationTrigger = new TaskCompletionSource<bool>();
            return m_animationTrigger.Task;
        }

        public void AnimationTrigger() => m_animationTrigger.SetResult(true);

        //public override void FireOnAnimationEndedEvent()
        //{
        //    if (CurrentAnimationState == m_attackAnimationID)
        //        OnAttackAnimationEnded?.Invoke(this, EventArgs.Empty);
        //}  
    }
}