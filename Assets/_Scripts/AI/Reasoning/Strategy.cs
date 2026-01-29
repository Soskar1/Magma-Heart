using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning.Plans;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        public List<PlanDefinition> Plans { get; init; } = new List<PlanDefinition>();

        public abstract float EvaluateState(Board boardState);
    }
}
