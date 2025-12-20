using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class EngageActionArgumentCreator : IActionArgumentCreator<EngageActionData>
    {
        public ActionArgs CreateArguments(EngageActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            return new EngageActionArgs(executor, target, data.Damage, data.Speed);
        }

        public ActionArgs CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((EngageActionData)data, executor, target, state);
    }
}
