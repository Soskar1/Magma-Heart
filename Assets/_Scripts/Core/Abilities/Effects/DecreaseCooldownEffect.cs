using MagmaHeart.Abilities.Effects;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record DecreaseCooldownEffect(int ExecutorId) : AbilityEffect(ExecutorId);
}
