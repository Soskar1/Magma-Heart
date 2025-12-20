using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Actions
{
    public interface IActionArgumentCreator
    {
        public ActionArgs CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state);
    }

    public interface IActionArgumentCreator<TData> : IActionArgumentCreator
        where TData : ActionData
    {
        public ActionArgs CreateArguments(TData data, AIUnitModel executor, AIUnitModel target, BoardState state);
    }
}
