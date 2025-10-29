namespace MagmaHeart.AI.Reasoning
{
    public class Strategy
    {
        public Action[] ActionsToConsider { get; init; }

        public Strategy(Action[] actionsToConsider) => ActionsToConsider = actionsToConsider; 
    }
}
