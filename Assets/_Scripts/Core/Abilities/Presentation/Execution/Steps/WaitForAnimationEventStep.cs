using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    public class WaitForAnimationEventStep : IAbilityExecutionStep
    {
        private readonly TimeSpan m_timeout;

        public WaitForAnimationEventStep(TimeSpan timeout)
        {
            m_timeout = timeout;
        }

        public async Task Run(AbilityExecutionContext context)
        {
            context.World.TryGetEntity(context.ExecutorId, out Entity executor);
            using var cancellationTokenSource = new CancellationTokenSource(m_timeout);
            await executor.Animation.WaitForAnimationEvent(cancellationTokenSource.Token);
        }
    }
}
