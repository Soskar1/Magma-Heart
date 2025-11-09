using System;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        // public Action[] ActionsToConsider { get; init; }
        public int LookAhead { get; init; }
        public Func<StateSnapshot, AIUnit> PlayerTargetSelection { get; init; }
        public AIUnit Player { get; init; }

        public Strategy(int lookAhead, Func<StateSnapshot, AIUnit> playerTargetSelection, AIUnit player)
        {
            // ActionsToConsider = actionsToConsider;
            LookAhead = lookAhead;
            PlayerTargetSelection = playerTargetSelection;
            Player = player;
        }

        public abstract float EvaluateState(StateSnapshot state);
    }
}
