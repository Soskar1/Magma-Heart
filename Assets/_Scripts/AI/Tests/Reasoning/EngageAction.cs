namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageAction : Action
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

        public override void Execute() { }

        public override bool CanSimulate(StateSnapshot state, AIUnit target)
        {
            Position possessorPosition = state.GetProperty<Position>(ActionPossessor);
            Position targetPosition = state.GetProperty<Position>(target);

            float distance = possessorPosition.Distance(targetPosition);
            if (distance > m_speed + 1 || distance <= 1)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            StateSnapshot moveState = m_moveAction.Simulate(state, target);
            return m_damageAction.Simulate(moveState, target);
        }
    }
}
