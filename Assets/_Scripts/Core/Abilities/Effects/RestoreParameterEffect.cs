using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record RestoreParameterEffect(int ExecutorId, ParameterId Parameter, float Amount) : AbilityEffect(ExecutorId);
}
