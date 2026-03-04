using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public enum AnimationType
    {
        Idle,
        Run,
        Attack
    }

    public sealed class EntityAnimation : AnimationPlayer
    {
        private readonly Dictionary<AnimationType, int> m_definedAnimations = new Dictionary<AnimationType, int>()
        {
            { AnimationType.Idle, Animator.StringToHash("Idle") },
            { AnimationType.Run, Animator.StringToHash("Run") },
            { AnimationType.Attack, Animator.StringToHash("Attack") },
        };

        private TaskCompletionSource<bool> m_animationTrigger;

        public void Initialize(RuntimeAnimatorController controller) => Animator.runtimeAnimatorController = controller;

        public void PlayAnimation(AnimationType animationType) => CurrentAnimationState = m_definedAnimations[animationType];
        
        public Task WaitForAnimationEvent(CancellationToken cancellationToken = default)
        {
            m_animationTrigger = new TaskCompletionSource<bool>();

            if (cancellationToken.CanBeCanceled)
                cancellationToken.Register(() => m_animationTrigger.TrySetResult(false));

            return m_animationTrigger.Task;
        }

        public Task WaitForAnimationEnd(AnimationType animationType, CancellationToken cancellationToken = default)
        {
            int stateHash = m_definedAnimations[animationType];
            return WaitForStateToFinish(stateHash, cancellationToken);
        }

        public void PlayIdleAnimation() => CurrentAnimationState = m_definedAnimations[AnimationType.Idle];
        public void PlayRunAnimation() => CurrentAnimationState = m_definedAnimations[AnimationType.Run];

        public int PlayAttackAnimation()
        {
            CurrentAnimationState = m_definedAnimations[AnimationType.Attack];
            return m_definedAnimations[AnimationType.Attack];
        }

        public Task GetAnimationTriggerTask()
        {
            m_animationTrigger = new TaskCompletionSource<bool>();
            return m_animationTrigger.Task;
        }


        public void AnimationTrigger() => m_animationTrigger.TrySetResult(true);
    }
}