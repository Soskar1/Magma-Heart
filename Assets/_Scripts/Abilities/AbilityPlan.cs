using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Resources;
using System.Collections.Generic;

namespace MagmaHeart.Abilities
{
    public record AbilityPlan(bool IsLegal, ResourceCost ComputedCost, IEnumerable<AbilityEffect> Effects);
}
