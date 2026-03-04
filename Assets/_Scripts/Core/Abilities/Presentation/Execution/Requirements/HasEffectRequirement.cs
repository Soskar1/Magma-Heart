using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Core.Abilities.Effects;
using System;
using System.Linq;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Requirements
{
    [Serializable]
    public abstract class HasEffectRequirement<T> : IExecutionRequirement where T : AbilityEffect
    {
        public bool IsMet(AbilityPlan plan) => plan.Effects.OfType<T>().Any();
    }

    [Serializable]
    public class HasDamageEffectRequirement : HasEffectRequirement<DamageEffect> { }

    [Serializable]
    public class HasMoveEffectRequirement : HasEffectRequirement<MoveEffect> { }
}
