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

            foreach (var parameter in totalCost.GetAllCosts())
            {
                float have = world.GetParameter(executorId, parameter.Id).CurrentValue;
                
                if (have < parameter.Amount)
                    return new AbilityPlan(ability, false, totalCost, new List<AbilityEffect>());
            }

            foreach (IAbilityRequirement requirement in ability.Requirements)
            {
                if (requirement == null)
                    continue;

                if (!requirement.IsMet(world, executorId, target))
                    return new AbilityPlan(ability, false, totalCost, new List<AbilityEffect>());
            }

            List<AbilityEffect> effects = BuildSpendCostEffects(executorId, totalCost);

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

            return new AbilityPlan(ability, true, totalCost, effects);
        }

        public List<AbilityEffect> BuildSpendCostEffects(int executorId, ResourceCost cost)
        {
            List<AbilityEffect> list = new List<AbilityEffect>();
            foreach (var parameter in cost.GetAllCosts())
                list.Add(new SpendResourceEffect(executorId, parameter.Id, parameter.Amount));
            
            return list;
        }
    }
}
