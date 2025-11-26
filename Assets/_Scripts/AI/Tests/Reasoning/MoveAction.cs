using MagmaHeart.AI.Actions;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveAction : UnitAction<MoveActionArgs>
    {
        public float m_speed;

        public MoveAction(AIUnitModel actionPossessor, float speed) : base(actionPossessor)
        {
            m_speed = speed;
        }

        public override IEnumerable<StateChange> ProduceChanges(MoveActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(ActionPossessor);

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
                new MovementStateChange(ActionPossessor, possessorPosition.CurrentPosition, tmpPosition)
            };
        }

        public override bool CanExecute(MoveActionArgs args, BoardState gameState)
        {
            Position possessorPosition = gameState.GetProperty<Position>(ActionPossessor);

            if (possessorPosition.Distance(args.Target) <= 1)
                return false;

            return true;
        }

        public override IEnumerable<ActionArgs> CreateSimulationArgument(SimulatedBoardState state, AIUnitModel unit)
        {
            Position position = state.GetProperty<Position>(unit);
            return new List<MoveActionArgs>() { new MoveActionArgs(position.CurrentPosition) };
        }
    }
}
