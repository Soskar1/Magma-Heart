using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    public interface IActionArgumentCreator
    {
        public IEnumerable<ActionArgs> CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state);
    }

    public interface IActionArgumentCreator<TData> : IActionArgumentCreator
        where TData : ActionData
    {
        public IEnumerable<ActionArgs> CreateArguments(TData data, AIUnitModel executor, AIUnitModel target, BoardState state);
    }
}
