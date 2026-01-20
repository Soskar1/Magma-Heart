using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveAction : UnitAction<MoveActionArgs, TargetPositionActionInput, MoveActionData>
    {
        public override IEnumerable<StateChange> ProduceChanges(MoveActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Input.Executor);

            Vector2 tmpPosition = possessorPosition.CurrentPosition;

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


            return new List<StateChange>
            {
                new MovementStateChange(args.Input.Executor, possessorPosition.CurrentPosition, tmpPosition)
            };
        }

        public override bool CanExecute(MoveActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Input.Executor);

            if (possessorPosition.Distance(args.TypedInput.Target) <= 1)
                return false;

            return true;
        }

        public override bool TryCreateArgs(TargetPositionActionInput input, MoveActionData data, BoardState boardState, out MoveActionArgs args)
        {
            MoveActionArgs candidate = new MoveActionArgs(input, data);

            if (!CanExecute(candidate, boardState))
            {
                args = null;
                return false;
            }

            args = candidate;
            return true;
        }

        public override bool TryGenerateArgs(AIUnitModel executor, MoveActionData data, BoardState boardState, out ActionArgs args)
        {
            foreach (AIUnitModel unit in boardState.Board.GetUnits())
            {
                if (unit == executor)
                    continue;

                Vector2 position = boardState.GetProperty<Position>(unit).CurrentPosition;
                TargetPositionActionInput input = new TargetPositionActionInput(executor, position);

                if (TryCreateArgs(input, data, boardState, out args))
                    return true;
            }

            args = null;
            return false;
        }
    }
}
