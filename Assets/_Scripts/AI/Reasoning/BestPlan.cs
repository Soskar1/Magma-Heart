using MagmaHeart.Abilities;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public record BestPlan(IEnumerable<AbilityPlan> ExecutedTasks);
}