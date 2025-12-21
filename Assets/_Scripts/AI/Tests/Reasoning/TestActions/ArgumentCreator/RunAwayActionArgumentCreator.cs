using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayActionArgumentCreator : IActionArgumentCreator<RunAwayActionData>
    {
        public ActionArgs CreateArguments(RunAwayActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            return new RunAwayActionArgs(executor, target, data.Speed);
        }

        public ActionArgs CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((RunAwayActionData)data, executor, target, state);
    }
}
