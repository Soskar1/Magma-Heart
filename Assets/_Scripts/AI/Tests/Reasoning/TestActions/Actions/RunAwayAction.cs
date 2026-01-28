using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayAction : UnitAction<RunAwayActionArgs, TargetEntityActionInput, RunAwayActionData>
    {
        public override IEnumerable<StateChange> ProduceChanges(RunAwayActionArgs args, BoardState boardState)
        {
            boardState.Board.TryGetUnit(args.Input.Executor.Id, out Entity executor);
            boardState.Board.TryGetUnit(args.TypedInput.Target.Id, out Entity target);

            Vector2 tmpPosition = executor.Position;

            Vector2 direction = -(target.Position - tmpPosition);
            float xMovement = args.RunAwayActionData.Speed;
            float yMovement = args.RunAwayActionData.Speed;

            if (direction.x > 0)
                tmpPosition.x += xMovement;
            else if (direction.x < 0)
                tmpPosition.x -= xMovement;

            if (direction.y > 0)
                tmpPosition.y += yMovement;
            else if (direction.y < 0)
                tmpPosition.y -= yMovement;

            return new List<StateChange>()
            {
                new MovementStateChange(args.Input.Executor.Id, executor.Position, tmpPosition)
            };
        }

        public override bool CanExecute(RunAwayActionArgs args, BoardState gameState) => true;

        public override bool TryCreateArgs(TargetEntityActionInput input, RunAwayActionData data, BoardState boardState, out RunAwayActionArgs args)
        {
            RunAwayActionArgs candidate = new RunAwayActionArgs(input, data);

            if (!CanExecute(candidate, boardState))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, RunAwayActionData data, BoardState boardState, out ActionArgs args)
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
