namespace MagmaHeart.AI.Reasoning
{
    public class Strategy
    {
        public IAction[] ActionsToConsider { get; init; }

        public Strategy(IAction[] actionsToConsider) => ActionsToConsider = actionsToConsider; 
    }
}
