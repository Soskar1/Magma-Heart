using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class EngageActionArgumentCreator : IActionArgumentCreator<EngageActionData>
    {
        public IEnumerable<ActionArgs> CreateArguments(EngageActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            yield return new EngageActionArgs(executor, target, data.Damage, data.Speed);
        }

        public IEnumerable<ActionArgs> CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((EngageActionData)data, executor, target, state);
    }
}
