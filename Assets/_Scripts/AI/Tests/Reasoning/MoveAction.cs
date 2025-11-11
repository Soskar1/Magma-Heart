using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class MoveAction : Action<MoveActionArgs>
    {
        public float m_speed;

        public MoveAction(AIUnit actionPossessor, float speed) : base(actionPossessor)
        {
            m_speed = speed;
        }

        public override void Execute(MoveActionArgs args) { }

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, MoveActionArgs args)
        {
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);

            if (possessorPosition.Distance(args.Target) <= 1)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, MoveActionArgs args)
        {
            StateSnapshot newState = base.Simulate(state, board, args);

            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);

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

            newState.Update(ActionPossessor, new Position(tmpPosition));

            return newState;
        }
    }
}
