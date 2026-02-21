using MagmaHeart.Abilities.Resources;

namespace MagmaHeart.Abilities.Effects
{
    public sealed record SpendResourceEffect(int ExecutorId, ResourceId Resource, int Amount) : AbilityEffect(ExecutorId);
}
