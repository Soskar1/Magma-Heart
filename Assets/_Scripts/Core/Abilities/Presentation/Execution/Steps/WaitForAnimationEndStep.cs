using MagmaHeart.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class WaitForAnimationEndStep : IAbilityExecutionStep
    {
        [SerializeField] private AnimationType m_animationType;
        [SerializeField] private float m_timeoutSeconds = 5f;

        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            context.World.TryGetEntity(context.ExecutorId, out Entity entity);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(m_timeoutSeconds));
            await entity.Animation.WaitForAnimationEnd(m_animationType, cts.Token);
        }
    }
}
