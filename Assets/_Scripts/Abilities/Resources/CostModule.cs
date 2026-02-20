using MagmaHeart.Abilities.Targeting;
using System;

namespace MagmaHeart.Abilities.Resources
{
    [Serializable]
    public abstract class CostModule
    {
        public abstract ResourceCost ComputeCost(IGameWorld gameWorld, int executorId, AbilityTarget target);
    }
}
