using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayAction : Action
    {
        public float m_speed;

        public RunAwayAction(AIUnit actionPossessor, float speed) : base(actionPossessor)
        {
            m_speed = speed;
        }

        public override void Execute() { }

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, AIUnit target) => true;

        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, AIUnit target)
        {
            StateSnapshot newState = base.Simulate(state, board, target);

            Position targetPosition = state.GetProperty<Position>(target);
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);

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

            newState.Update(ActionPossessor, new Position(tmpPosition));

            return newState;
        }
    }
}
