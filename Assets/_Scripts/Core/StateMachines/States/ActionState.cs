using System.Collections.Generic;

namespace MagmaHeart.Core.StateMachines
{
    public class ActionState : IState
    {
        private readonly List<IActionStateListener> m_listeners;

        public ActionState(List<IActionStateListener> listeners) => m_listeners = listeners;

        public void Enter(params object[] args)
        {
            foreach (IActionStateListener listener in m_listeners)
                listener.EnterActionState();
        }

        public void Exit()
        {
            foreach (IActionStateListener listener in m_listeners)
                listener.ExitActionState();
        }
    }
}