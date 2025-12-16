using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class RunAwayActionArgumentCreator : IActionArgumentCreator<RunAwayActionData>
    {
        public IEnumerable<ActionArgs> CreateArguments(RunAwayActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            yield return new RunAwayActionArgs(executor, target, data.Speed);
        }

        public IEnumerable<ActionArgs> CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((RunAwayActionData)data, executor, target, state);
    }
}
