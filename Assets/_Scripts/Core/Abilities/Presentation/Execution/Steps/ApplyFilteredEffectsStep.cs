using MagmaHeart.Abilities.Effects;
using MagmaHeart.Core.Abilities.Effects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [Serializable]
    public abstract class ApplyFilteredEffectsStep<T> : IAbilityExecutionStep where T : AbilityEffect
    {
        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            foreach (T effect in context.Plan.Effects.OfType<T>())
                context.EffectDispatcher.Apply(context.World, effect);

            return Task.CompletedTask;
        }
    }

    [Serializable]
    public class ApplyDamageStep : ApplyFilteredEffectsStep<DamageEffect> { }

    [Serializable]
    public class SpendResourceStep : ApplyFilteredEffectsStep<SpendResourceEffect> { }
}
