using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class WaitForAnimationEventStep : IAbilityExecutionStep
    {
        [SerializeField] private float m_timeoutSeconds = 5f;

        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            context.World.TryGetEntity(context.ExecutorId, out Entity executor);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(m_timeoutSeconds));
            await executor.Animation.WaitForAnimationEvent(cts.Token);
        }
    }
}
