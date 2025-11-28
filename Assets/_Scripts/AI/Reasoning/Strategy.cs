using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        // public Action[] ActionsToConsider { get; init; }
        public int LookAhead { get; init; }
        public AIUnitModel Player { get; init; }

        public Strategy(int lookAhead, AIUnitModel player)
        {
            // ActionsToConsider = actionsToConsider;
            LookAhead = lookAhead;
            Player = player;
        }

        public abstract float EvaluateState(SimulatedBoardState boardState);
    }
}
