namespace MagmaHeart.Core.StateMachines
{
    public class StateMachine
    {
        private IState m_currentState;

        public StateMachine(IState startState) => m_currentState = startState;

        public void ChangeState(IState state, params object[] args)
        {
            m_currentState?.Exit();
            m_currentState = state;
            m_currentState.Enter(args);
        }
    }
}