using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveActionArgumentCreator : IActionArgumentCreator<MoveActionData>
    {
        public ActionArgs CreateArguments(MoveActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            Position position = state.GetProperty<Position>(target);
            return new MoveActionArgs(executor, position.CurrentPosition, data.Speed);
        }

        public ActionArgs CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((MoveActionData)data, executor, target, state);
    }
}
