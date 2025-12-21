using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions.ArgumentCreators
{
    public class AttackActionArgumentCreator : IActionArgumentCreator<AttackActionData>
    {
        public ActionArgs CreateArguments(AttackActionData data, AIUnitModel executor, AIUnitModel target, BoardState state)
        {
            return new AttackActionArgs((EntityModel)executor, (EntityModel)target, data.EnergyCost, data.AttackDistance, data.AttackDamage, data.AttackType);
        }

        public ActionArgs CreateArguments(ActionData data, AIUnitModel executor, AIUnitModel target, BoardState state) => CreateArguments((AttackActionData)data, executor, target, state);
    }
}
