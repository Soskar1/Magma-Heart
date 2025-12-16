using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class AttackActionArgumentCreator : IActionArgumentCreator<AttackActionData>
    {
        public IEnumerable<ActionArgs> CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((AttackActionData)data, executor, target, state);

        public IEnumerable<ActionArgs> CreateArguments(AttackActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            yield return new AttackActionArgs(executor, target, data.Damage);
        }
    }
}
