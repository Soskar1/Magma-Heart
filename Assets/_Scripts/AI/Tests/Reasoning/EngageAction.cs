using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageAction : Action<EngageActionArgs>
    {
        private MoveAction m_moveAction;
        private AttackAction m_damageAction;
        private float m_speed;

        public EngageAction(AIUnit actionPossessor, float damage, float speed) : base(actionPossessor)
        {
            m_speed = speed;
            m_moveAction = new MoveAction(actionPossessor, speed);
            m_damageAction = new AttackAction(actionPossessor, damage);
        }

        public override void Execute(EngageActionArgs args) { }

        public override ActionArgs CreateActionArgs(StateSnapshot state, AIUnit unit) => new EngageActionArgs(unit);

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, EngageActionArgs args)
        {
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);
            Position targetPosition = state.GetProperty<Position>(args.Target);

            float distance = possessorPosition.Distance(targetPosition);
            if (distance > m_speed + 1 || distance <= 1)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, EngageActionArgs args)
        {
            Position targetPosition = state.GetProperty<Position>(args.Target);

            MoveActionArgs moveArgs = new MoveActionArgs(targetPosition.CurrentPosition);
            StateSnapshot moveState = m_moveAction.Simulate(state, board, moveArgs);

            AttackActionArgs attackArgs = new AttackActionArgs(args.Target);
            return m_damageAction.Simulate(moveState, board, attackArgs);
        }

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, ActionArgs args) => CanSimulate(state, board, (EngageActionArgs)args);
        public override void Execute(ActionArgs args) => Execute((EngageActionArgs)args);
    }
}
