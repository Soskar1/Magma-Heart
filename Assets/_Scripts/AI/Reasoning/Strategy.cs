namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy
    {
        // public Action[] ActionsToConsider { get; init; }
        public int LookAhead { get; init; }

        public Strategy(int lookAhead)
        {
            // ActionsToConsider = actionsToConsider;
            LookAhead = lookAhead;
        }

        public abstract float EvaluateState(StateSnapshot state);
    }
}
