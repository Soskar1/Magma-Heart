using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AttackAction : UnitAction<AttackActionArgs, TargetEntityActionInput, AttackActionData>
    {
        public override bool CanExecute(AttackActionArgs args, Board board)
        {
            board.TryGetUnit(args.Input.Executor.Id, out Entity executor);
            board.TryGetUnit(args.TypedInput.Target.Id, out Entity target);

            if (Vector2.Distance(executor.Position, target.Position) > 1)
                return false;

            return true;
        }

        public override IEnumerable<IBoardCommand> Execute(AttackActionArgs args, Board gameState)
        {
            return new List<IBoardCommand> {
                new ApplyDamageStateChange(args.AttackActionData.Damage, args.TypedInput.Target.Id)
            };
        }

        public override bool TryCreateArgs(TargetEntityActionInput input, AttackActionData data, Board board, out AttackActionArgs args)
        {
            AttackActionArgs candidate = new AttackActionArgs(input, data);

            if (!CanExecute(candidate, board))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, AttackActionData data, Board board, out ActionArgs args)
        {
            foreach (AIUnitModel unit in board.GetUnits())
            {
                if (unit.Id == executor.Id)
                    continue;

                TargetEntityActionInput input = new TargetEntityActionInput(executor, unit);
                if (TryCreateArgs(input, data, board, out args))
                    return true;
            }

            args = null;
            return false;
        }
    }
}
