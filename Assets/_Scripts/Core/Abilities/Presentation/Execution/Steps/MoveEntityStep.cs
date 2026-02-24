using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    public class MoveEntityStep : IAbilityExecutionStep
    {
        private readonly List<MoveEffect> m_moveEffects;

        public MoveEntityStep(List<MoveEffect> moveEffects)
        {
            m_moveEffects = moveEffects;
        }

        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            context.World.TryGetEntity(context.ExecutorId, out Entity entity);
            
            foreach (MoveEffect effect in m_moveEffects)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await entity.TileBasedMovement.StartMovementAsync(effect.Path);
            }
        }
    }
}
