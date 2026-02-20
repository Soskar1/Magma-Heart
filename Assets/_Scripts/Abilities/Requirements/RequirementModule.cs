using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using UnityEngine;

namespace MagmaHeart.Abilities.Requirements
{
    public abstract class RequirementModule : ScriptableObject
    {
        public abstract bool IsMet(IGameWorld gameWorld, int executorId, AbilityTarget target, ResourceCost computedCost);
    }
}
