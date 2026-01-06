using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : UnitAction<AttackActionArgs, TargetEntityActionInput, AttackActionData>
    {
        public override bool CanExecute(AttackActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Input.Executor);
            Position targetPosition = gameState.GetProperty<Position>(args.TypedInput.Target);

            if (possessorPosition.Distance(targetPosition) > 1)
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState gameState)
        {
            return new List<StateChange> {
                new ApplyDamageStateChange(args.AttackActionData.Damage, args.TypedInput.Target)
            };
        }

        public override bool TryCreateArgs(TargetEntityActionInput input, AttackActionData data, BoardState boardState, out AttackActionArgs args)
        {
            AttackActionArgs candidate = new AttackActionArgs(input, data);

            if (!CanExecute(candidate, boardState))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, AttackActionData data, BoardState boardState, out AttackActionArgs args)
        {
            foreach (AIUnitModel unit in boardState.Board.GetUnits())
            {
                if (unit == executor)
                    continue;

                TargetEntityActionInput input = new TargetEntityActionInput(executor, unit);
                if (TryCreateArgs(input, data, boardState, out args))
                    return true;
            }

            args = null;
            return false;
        }
    }
}
