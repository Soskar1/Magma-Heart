using System;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        // public Action[] ActionsToConsider { get; init; }
        public int LookAhead { get; init; }

        // TODO: Maybe we can move this to the AIUnit class?
        // So, that every unit could have it's target selection function
        // In that way, enemies will not be able to target only the player
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
