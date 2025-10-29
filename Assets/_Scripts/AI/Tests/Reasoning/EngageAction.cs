namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class EngageAction : IAction
    {
        private MoveAction m_moveAction;
        private AttackAction m_damageAction;
        private float m_speed;

        public AIUnit ActionPossessor { get; }

        public EngageAction(AIUnit actionPossessor, float damage, float speed)
        {
            m_speed = speed;
            ActionPossessor = actionPossessor;
            m_moveAction = new MoveAction(actionPossessor, speed);
            m_damageAction = new AttackAction(actionPossessor, damage);
        }

        public void Execute() { }

        public bool CanSimulate(StateSnapshot state, AIUnit target)
        {
            Position possessorPosition = (Position)state.GetProperty(ActionPossessor, typeof(Position));
            Position targetPosition = (Position)state.GetProperty(target, typeof(Position));

            if (possessorPosition.Distance(targetPosition) > m_speed)
                return false;

            return true;
        }

        public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            StateSnapshot moveState = m_moveAction.Simulate(state, target);
            return m_damageAction.Simulate(moveState, target);
        }
    }
}
