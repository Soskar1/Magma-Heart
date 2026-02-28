using MagmaHeart.Abilities;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Presentation.Execution
{
    public sealed record AbilityExecutionContext(GameWorld World, int ExecutorId, EffectDispatcher EffectDispatcher, AbilityPlan Plan);
}
