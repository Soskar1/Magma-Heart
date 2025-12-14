using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        // public Action[] ActionsToConsider { get; init; }
        public AIUnitModel Player { get; init; }

        public Strategy(AIUnitModel player)
        {
            // ActionsToConsider = actionsToConsider;
            Player = player;
        }

        public abstract float EvaluateState(SimulatedBoardState boardState);
    }
}
