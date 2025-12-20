using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class AttackActionArgumentCreator : IActionArgumentCreator<AttackActionData>
    {
        public ActionArgs CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((AttackActionData)data, executor, target, state);

        public ActionArgs CreateArguments(AttackActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            return new AttackActionArgs(executor, target, data.Damage);
        }
    }
}
