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
                float have = world.GetResource(executorId, resource.ResourceId);
                
                if (have < resource.Amount)
                    return new AbilityPlan(false, totalCost, new List<AbilityEffect>());
            }

            foreach (IAbilityRequirement requirement in ability.Requirements)
            {
                if (requirement == null)
                    continue;

                if (!requirement.IsMet(world, executorId, target))
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

        public List<AbilityEffect> BuildSpendCostEffects(int executorId, ResourceCost cost)
        {
            List<AbilityEffect> list = new List<AbilityEffect>();
            foreach (var resource in cost.GetAllCosts())
                list.Add(new SpendResourceEffect(executorId, resource.ResourceId, resource.Amount));
            
            return list;
        }
    }
}
