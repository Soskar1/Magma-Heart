using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveAction : UnitAction<MoveActionArgs>
    {
        public float m_speed;

        public MoveAction(float speed)
        {
            m_speed = speed;
        }

        public override IEnumerable<StateChange> ProduceChanges(MoveActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);

            Vector2 tmpPosition = possessorPosition.CurrentPosition;

            Vector2 direction = args.Target - tmpPosition;
            float xMovement = Mathf.Min(Mathf.Abs(direction.x), m_speed);
            float yMovement = Mathf.Min(Mathf.Abs(direction.y), m_speed);

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

        public override bool CanExecute(MoveActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(args.Executor);

            if (possessorPosition.Distance(args.Target) <= 1)
                return false;

            return true;
        }

        public override IEnumerable<ActionArgs> CreateSimulationArguments(SimulatedBoardState state, AIUnitModel executor, IEnumerable<AIUnitModel> targets)
        {
            foreach (AIUnitModel unit in targets)
            {
                Position position = state.GetProperty<Position>(unit);
                yield return new MoveActionArgs(executor, position.CurrentPosition);
            }
        }
    }
}
