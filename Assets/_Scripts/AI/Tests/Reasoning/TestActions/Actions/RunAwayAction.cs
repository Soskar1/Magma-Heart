using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayAction : UnitAction<RunAwayActionArgs>
    {
        public override IEnumerable<StateChange> ProduceChanges(RunAwayActionArgs args, BoardState gameState)
        {
            Position targetPosition = gameState.GetProperty<Position>(args.RunAwayFrom);
            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);

            Vector2 tmpPosition = possessorPosition.CurrentPosition;

            Vector2 direction = -(targetPosition.CurrentPosition - tmpPosition);
            float xMovement = args.Speed;
            float yMovement = args.Speed;

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

        public override bool CanExecute(RunAwayActionArgs args, BoardState gameState) => true;
    }
}
