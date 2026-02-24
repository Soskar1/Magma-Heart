using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    public class WaitForAnimationEndStep : IAbilityExecutionStep
    {
        private readonly TimeSpan m_timeout;
        private readonly AnimationType m_animationType;

        public WaitForAnimationEndStep(AnimationType animationType, TimeSpan timeout)
        {
            m_timeout = timeout;
            m_animationType = animationType;
        }

        public async Task Run(AbilityExecutionContext context)
        {
            context.World.TryGetEntity(context.ExecutorId, out Entity entity);
            using var cancellationTokenSource = new CancellationTokenSource(m_timeout);
            await entity.Animation.WaitForAnimationEnd(m_animationType, cancellationTokenSource.Token);
        }
    }
}
