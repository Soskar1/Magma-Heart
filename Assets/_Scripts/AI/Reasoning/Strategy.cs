using System;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        // public Action[] ActionsToConsider { get; init; }
        public int LookAhead { get; init; }
        public Func<StateSnapshot, AIUnit> PlayerTargetSelection { get; init; }

        public Strategy(int lookAhead, Func<StateSnapshot, AIUnit> playerTargetSelection)
        {
            // ActionsToConsider = actionsToConsider;
            LookAhead = lookAhead;
            PlayerTargetSelection = playerTargetSelection;
        }

        public abstract float EvaluateState(StateSnapshot state);
    }
}
