using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        // public Action[] ActionsToConsider { get; init; }
        public int LookAhead { get; init; }
        public AIUnit Player { get; init; }

        public Strategy(int lookAhead, AIUnit player)
        {
            // ActionsToConsider = actionsToConsider;
            LookAhead = lookAhead;
            Player = player;
        }

        public abstract float EvaluateState(SimulatedBoardState boardState);
    }
}
