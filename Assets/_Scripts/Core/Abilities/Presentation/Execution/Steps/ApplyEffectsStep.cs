using MagmaHeart.Abilities.Effects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    public class ApplyEffectsStep : IAbilityExecutionStep
    {
        private readonly IEnumerable<AbilityEffect> m_effects;

        public ApplyEffectsStep(IEnumerable<AbilityEffect> effects)
        {
            m_effects = effects;
        }

        public Task Run(AbilityExecutionContext context)
        {
            foreach (AbilityEffect effect in m_effects)
                context.EffectDispatcher.Apply(context.World, effect);

            return Task.CompletedTask;
        }
    }
}
