using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class PlayAnimationStep : IAbilityExecutionStep
    {
        [SerializeField] private AnimationType m_animationType;

        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            context.World.TryGetEntity(context.ExecutorId, out Entity executor);
            executor.Animation.PlayAnimation(m_animationType);

            return Task.CompletedTask;
        }
    }
}
