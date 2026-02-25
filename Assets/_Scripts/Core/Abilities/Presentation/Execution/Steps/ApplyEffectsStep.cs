using MagmaHeart.Abilities.Effects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public class ApplyEffectsStep : IAbilityExecutionStep
    {
        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            foreach (AbilityEffect effect in context.Plan.Effects)
                context.EffectDispatcher.Apply(context.World, effect);

            return Task.CompletedTask;
        }
    }
}
