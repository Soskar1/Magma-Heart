using MagmaHeart.AI.Reasoning.Plans;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public record BestPlan(IEnumerable<ExecutedTask> ExecutedTasks);
}