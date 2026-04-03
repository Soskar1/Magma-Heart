using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    public enum StepTarget
    {
        Executor,
        Target
    }

    [Serializable]
    public class PlayAnimationStep : IAbilityExecutionStep
    {
        [SerializeField] private AnimationType m_animationType;
        [SerializeField] private StepTarget m_target = StepTarget.Executor;

        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            var targetEntityId = m_target == StepTarget.Executor ? context.ExecutorId : context.Plan.Target.EntityId;
            context.World.TryGetEntity(targetEntityId, out Entity entity);
            
            if (entity == null)
                return Task.CompletedTask;

            entity.Animation.PlayAnimation(m_animationType);

            return Task.CompletedTask;
        }
    }
}
