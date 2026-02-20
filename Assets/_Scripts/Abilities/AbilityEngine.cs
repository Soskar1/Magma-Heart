using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Requirements;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;

namespace MagmaHeart.Abilities
{
    public sealed class AbilityEngine
    {
        public AbilityPlan Plan(IGameWorld world, int executorId, AbilityDefinition ability, AbilityTarget target)
        {
            if (ability.Targeting != null && !ability.Targeting.ValidateChosenTarget(world, executorId, target))
                return new AbilityPlan(false, ResourceCost.Zero, new List<AbilityEffect>());

            ResourceCost cost = ability.Cost != null ? ability.Cost.ComputeCost(world, executorId, target) : ResourceCost.Zero;

            if (ability.Requirements != null)
            {
                foreach (RequirementModule requirement in ability.Requirements)
                {
                    if (requirement == null)
                        continue;

                    bool isMet = requirement.IsMet(world, executorId, target, cost);
                    if (!isMet)
                        return new AbilityPlan(false, cost, new List<AbilityEffect>());
                }
            }

            List<AbilityEffect> effects = new List<AbilityEffect>();

            if (ability.Effects != null)
            {
                foreach (EffectModule effectModule in ability.Effects)
                {
                    if (effectModule == null)
                        continue;

                    IEnumerable<AbilityEffect> moduleEffects = effectModule.BuildEffects(world, executorId, target);
                    effects.AddRange(moduleEffects);
                }
            }

            return new AbilityPlan(true, cost, effects);
        }
    }
}
