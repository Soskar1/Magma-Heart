using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning.Tests.StateChanges;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayAction : UnitAction<RunAwayActionArgs>
    {
        public float m_speed;

        public RunAwayAction(AIUnit actionPossessor, float speed) : base(actionPossessor)
        {
            m_speed = speed;
        }

        public override List<StateChange> ProduceChanges(RunAwayActionArgs args, BoardState gameState)
        {
            Position targetPosition = gameState.GetProperty<Position>(args.RunAwayFrom);
            Position possessorPosition = gameState.GetProperty<Position>(ActionPossessor);

            Vector2 tmpPosition = possessorPosition.CurrentPosition;

            Vector2 direction = -(targetPosition.CurrentPosition - tmpPosition);
            float xMovement = Mathf.Abs(direction.x) * m_speed;
            float yMovement = Mathf.Abs(direction.y) * m_speed;

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
                new MovementStateChange(ActionPossessor, possessorPosition.CurrentPosition, tmpPosition)
            };
        }

        public override bool CanExecute(RunAwayActionArgs args, BoardState gameState) => true;

        public override ActionArgs CreateArgument(BoardState state, AIUnit unit) => new RunAwayActionArgs(unit);
    }
}
