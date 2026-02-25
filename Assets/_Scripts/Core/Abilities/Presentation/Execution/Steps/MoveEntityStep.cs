using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class MoveEntityStep : IAbilityExecutionStep
    {
        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            context.World.TryGetEntity(context.ExecutorId, out Entity entity);

            List<MoveEffect> moveEffects = context.Plan.Effects
                .OfType<MoveEffect>()
                .ToList();

            foreach (MoveEffect effect in moveEffects)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await entity.TileBasedMovement.StartMovementAsync(effect.Path);
            }
        }
    }
}
