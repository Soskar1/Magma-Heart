using MagmaHeart.Abilities.Resources;

namespace MagmaHeart.Abilities
{
    public sealed record ActionEvaluation(bool IsLegal, ResourceCost ComputedCost);
}
