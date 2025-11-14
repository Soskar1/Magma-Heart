using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class RunAwayAction : Action<RunAwayActionArgs>
    {
        public float m_speed;

        public RunAwayAction(AIUnit actionPossessor, float speed) : base(actionPossessor)
        {
            m_speed = speed;
        }

        public override void Execute(RunAwayActionArgs args) { }

        public override ActionArgs CreateActionArgs(StateSnapshot state, SimulatedBoard board, AIUnit unit) => new RunAwayActionArgs(unit);

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, RunAwayActionArgs args) => true;

        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, RunAwayActionArgs args)
        {
            StateSnapshot newState = base.Simulate(state, board, args);

            Position targetPosition = state.GetProperty<Position>(args.RunAwayFrom);
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

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, ActionArgs args) => CanSimulate(state, board, (RunAwayActionArgs)args);
        public override void Execute(ActionArgs args) => Execute((RunAwayActionArgs)args);
    }
}
