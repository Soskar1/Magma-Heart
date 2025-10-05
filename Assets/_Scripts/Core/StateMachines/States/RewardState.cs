using System.Collections.Generic;

namespace MagmaHeart.Core.StateMachines
{
    public class RewardState : IState
    {
        private readonly List<IRewardStateListener> m_listeners;

        public RewardState(List<IRewardStateListener> listeners) => m_listeners = listeners;

        public void Enter(params object[] args)
        {
            foreach (IRewardStateListener listener in m_listeners)
                listener.EnterRewardState();
        }

        public void Exit()
        {
            foreach (IRewardStateListener listener in m_listeners)
                listener.ExitRewardState();
        }
    }
}