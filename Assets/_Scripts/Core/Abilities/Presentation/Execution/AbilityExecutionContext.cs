using MagmaHeart.Abilities;
using MagmaHeart.Core.Abilities.Effects;

namespace MagmaHeart.Core.Abilities.Presentation.Execution
{
    public sealed record AbilityExecutionContext(GameWorld World, int ExecutorId, EffectDispatcher EffectDispatcher, AbilityPlan Plan);
}
