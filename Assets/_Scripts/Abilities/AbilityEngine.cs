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
            if (target == AbilityTarget.None)
                return new AbilityPlan(ability, false, ResourceCost.Zero, new List<AbilityEffect>(), target);

            if (world.GetCooldown(executorId, ability.Id) > 0)
                return new AbilityPlan(ability, false, ResourceCost.Zero, new List<AbilityEffect>(), target);

            ResourceCost totalCost = ability.ComputeCost(world, executorId, target);

            foreach (var parameter in totalCost.GetAllCosts())
            {
                float have = world.GetParameter(executorId, parameter.Id).CurrentValue;
                
                if (have < parameter.Amount)
                    return new AbilityPlan(ability, false, totalCost, new List<AbilityEffect>(), target);
            }

            foreach (IAbilityRequirement requirement in ability.Requirements)
            {
                if (requirement == null)
                    continue;

                if (!requirement.IsMet(world, executorId, target))
                    return new AbilityPlan(ability, false, totalCost, new List<AbilityEffect>(), target);
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

            return new AbilityPlan(ability, true, totalCost, effects, target);
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
