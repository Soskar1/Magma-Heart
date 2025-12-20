using MagmaHeart.AI.Plans;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        public AIUnitModel Player { get; init; }
        public List<PlanDefinition> Plans { get; init; } = new List<PlanDefinition>();

        public Strategy(AIUnitModel player)
        {
            Player = player;
        }

        public abstract float EvaluateState(SimulatedBoardState boardState);
    }
}
