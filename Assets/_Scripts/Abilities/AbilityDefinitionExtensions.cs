using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;

namespace MagmaHeart.Abilities
{
    public static class AbilityDefinitionExtensions
    {
        public static ResourceCost ComputeCost(this AbilityDefinition ability, IGameWorld world, int executorId, AbilityTarget target)
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

            return totalCost;
        }
    }
}
