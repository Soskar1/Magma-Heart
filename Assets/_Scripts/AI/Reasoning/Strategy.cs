using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        public List<PlanDefinition> Plans { get; init; } = new List<PlanDefinition>();

        public abstract float EvaluateState(SimulatedBoardState boardState);
    }
}
