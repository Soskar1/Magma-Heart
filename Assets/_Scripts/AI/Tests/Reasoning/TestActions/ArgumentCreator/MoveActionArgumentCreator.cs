using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class MoveActionArgumentCreator : IActionArgumentCreator<MoveActionData>
    {
        public IEnumerable<ActionArgs> CreateArguments(MoveActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            Position position = state.GetProperty<Position>(target);
            yield return new MoveActionArgs(executor, position.CurrentPosition, data.Speed);
        }

        public IEnumerable<ActionArgs> CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((MoveActionData)data, executor, target, state);
    }
}
