using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;

namespace MagmaHeart.Abilities
{
    public record AbilityPlan(
        AbilityDefinition AbilityDefinition,
        bool IsLegal,
        ResourceCost ComputedCost,
        IEnumerable<AbilityEffect> Effects,
        AbilityTarget Target);
}
