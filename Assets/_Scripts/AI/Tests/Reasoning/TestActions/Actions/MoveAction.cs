using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveAction : UnitAction
    {
        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState gameState)
        {
            MoveActionArgs moveActionArgs = args as MoveActionArgs;
            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);

            Vector2 tmpPosition = possessorPosition.CurrentPosition;

            Vector2 direction = moveActionArgs.Target - tmpPosition;
            float xMovement = Mathf.Min(Mathf.Abs(direction.x), moveActionArgs.MoveActionData.Speed);
            float yMovement = Mathf.Min(Mathf.Abs(direction.y), moveActionArgs.MoveActionData.Speed);

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
                new MovementStateChange(args.Executor, possessorPosition.CurrentPosition, tmpPosition)
            };
        }

        public override bool CanExecute(ActionArgs args, BoardState gameState)
        {
            MoveActionArgs moveActionArgs = args as MoveActionArgs;
            Position possessorPosition = gameState.GetProperty<Position>(moveActionArgs.Executor);

            if (possessorPosition.Distance(moveActionArgs.Target) <= 1)
                return false;

            return true;
        }
    }
}
