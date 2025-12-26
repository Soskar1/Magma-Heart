using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : UnitAction
    {
        public override bool CanExecute(ActionArgs args, BoardState gameState)
        {
            AttackActionArgs attackActionArgs = args as AttackActionArgs;

            Position possessorPosition = gameState.GetProperty<Position>(attackActionArgs.Executor);
            Position targetPosition = gameState.GetProperty<Position>(attackActionArgs.Target);

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState gameState)
        {
            AttackActionArgs attackActionArgs = args as AttackActionArgs;

            return new List<StateChange> {
                new ApplyDamageStateChange(attackActionArgs.AttackActionData.Damage, attackActionArgs.Target)
            };
        }
    }
}
