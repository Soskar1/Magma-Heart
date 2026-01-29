using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveAction : UnitAction<MoveActionArgs, TargetPositionActionInput, MoveActionData>
    {
        public override IEnumerable<IBoardCommand> Execute(MoveActionArgs args, Board board)
        {
            board.TryGetUnit(args.Input.Executor.Id, out Entity executorUnit);

            Vector2 tmpPosition = executorUnit.Position;

            Vector2 direction = args.TypedInput.Target - tmpPosition;
            float xMovement = Mathf.Min(Mathf.Abs(direction.x), args.MoveActionData.Speed);
            float yMovement = Mathf.Min(Mathf.Abs(direction.y), args.MoveActionData.Speed);

            if (direction.x > 0)
                tmpPosition.x += xMovement;
            else if (direction.x < 0)
                tmpPosition.x -= xMovement;

            if (direction.y > 0)
                tmpPosition.y += yMovement;
            else if (direction.y < 0)
                tmpPosition.y -= yMovement;


            return new List<IBoardCommand>
            {
                new MoveCommand(args.Input.Executor.Id, executorUnit.Position, tmpPosition)
            };
        }

        public override bool CanExecute(MoveActionArgs args, Board board)
        {
            board.TryGetUnit(args.Input.Executor.Id, out Entity executorUnit);

            if (Vector2.Distance(executorUnit.Position, args.TypedInput.Target) <= 1)
                return false;

            return true;
        }

        public override bool TryCreateArgs(TargetPositionActionInput input, MoveActionData data, Board board, out MoveActionArgs args)
        {
            MoveActionArgs candidate = new MoveActionArgs(input, data);

            if (!CanExecute(candidate, board))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, MoveActionData data, Board board, out ActionArgs args)
        {
            foreach (AIUnitModel unit in board.GetUnits())
            {
                if (unit.Id == executor.Id)
                    continue;

                board.TryGetUnit(unit.Id, out Entity unitEntity);

                TargetPositionActionInput input = new TargetPositionActionInput(executor, unitEntity.Position);

                if (TryCreateArgs(input, data, board, out args))
                    return true;
            }

            args = null;
            return false;
        }
    }
}
