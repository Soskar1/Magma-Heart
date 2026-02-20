using MagmaHeart.Abilities.Effects;
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

            ResourceCost totalCost = ResourceCost.Zero;

            if (ability.Cost != null)
            {
                foreach (CostModule cost in ability.Cost)
                {
                    if (cost == null)
                        continue;

                    ResourceCost moduleCost = cost.ComputeCost(world, executorId, target);
                    totalCost.Add(moduleCost);
                }
            }

            foreach (var resource in totalCost.GetAllCosts())
            {
                var have = world.GetResource(executorId, resource.ResourceId);
                
                if (have < resource.Amount)
                    return new AbilityPlan(false, totalCost, new List<AbilityEffect>());
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

            return new AbilityPlan(true, totalCost, effects);
        }
    }
}
