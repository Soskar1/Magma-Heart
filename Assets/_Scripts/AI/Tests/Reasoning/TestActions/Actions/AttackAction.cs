using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : UnitAction<AttackActionArgs, TargetEntityActionInput, AttackActionData>
    {
        public override bool CanExecute(AttackActionArgs args, BoardState boardState)
        {
            Position possessorPosition = boardState.GetProperty<Position>(args.Input.Executor);
            Position targetPosition = boardState.GetProperty<Position>(args.TypedInput.Target);

            boardState.Board.TryGetUnit(args.Input.Executor.Id, out Entity executor);
            boardState.Board.TryGetUnit(args.TypedInput.Target.Id, out Entity target);

            if (Vector2.Distance(executor.Position, target.Position) > 1)
                return false;

            return true;
        }

        public override IEnumerable<StateChange> ProduceChanges(AttackActionArgs args, BoardState gameState)
        {
            return new List<StateChange> {
                new ApplyDamageStateChange(args.AttackActionData.Damage, args.TypedInput.Target.Id)
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

        public override bool TryGenerateArgs(AIUnitModel executor, AttackActionData data, BoardState boardState, out ActionArgs args)
        {
            foreach (AIUnitModel unit in boardState.Board.GetUnits())
            {
                if (unit.Id == executor.Id)
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
