using MagmaHeart.Abilities.Targeting;
using UnityEngine;

namespace MagmaHeart.Abilities.Resources
{
    public abstract class CostModule : ScriptableObject
    {
        public abstract ResourceCost ComputeCost(IGameWorld gameWorld, int executorId, AbilityTarget target);
    }
}
