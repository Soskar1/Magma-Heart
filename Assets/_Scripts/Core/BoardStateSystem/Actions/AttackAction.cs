using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using MagmaHeart.Core.Entities.Properties;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackAction : CombatAction
    {
        public override int GetEnergyCost(ActionArgs args, BoardState gameState)
        {
            AttackActionArgs attackArgs = args as AttackActionArgs;
            return attackArgs.AttackActionData.EnergyCost;
        }

        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState gameState)
        {
            AttackActionArgs attackArgs = args as AttackActionArgs;
            IEnumerable<StateChange> changes = base.ProduceChanges(args, gameState);

            return changes.Concat(new List<StateChange>
            {
                new AttackStateChange(attackArgs.TypedExecutor, attackArgs.Target, attackArgs.AttackActionData.AttackDamage, attackArgs.AttackActionData.AttackType),
            });
        }

        public override bool CanExecute(ActionArgs args, BoardState gameState)
        {
            AttackActionArgs attackArgs = args as AttackActionArgs;

            bool result = base.CanExecute(args, gameState);
            if (!result)
                return result;

            PositionPropertySnapshot possessorPosition = gameState.GetProperty<PositionPropertySnapshot>(attackArgs.TypedExecutor);
            PositionPropertySnapshot targetPosition = gameState.GetProperty<PositionPropertySnapshot>(attackArgs.Target);

            if (possessorPosition.ManhattanDistance(targetPosition) > attackArgs.AttackActionData.AttackDistance)
                return false;

            return true;
        }
    }
}