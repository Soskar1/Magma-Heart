using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayAction : UnitAction
    {
        public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState gameState)
        {
            RunAwayActionArgs runAwayArgs = args as RunAwayActionArgs;

            Position targetPosition = gameState.GetProperty<Position>(runAwayArgs.RunAwayFrom);
            Position possessorPosition = gameState.GetProperty<Position>(runAwayArgs.Executor);

            Vector2 tmpPosition = possessorPosition.CurrentPosition;

            Vector2 direction = -(targetPosition.CurrentPosition - tmpPosition);
            float xMovement = runAwayArgs.RunAwayActionData.Speed;
            float yMovement = runAwayArgs.RunAwayActionData.Speed;

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
                new MovementStateChange(args.Executor, possessorPosition.CurrentPosition, tmpPosition)
            };
        }

        public override bool CanExecute(ActionArgs args, BoardState gameState) => true;
    }
}
