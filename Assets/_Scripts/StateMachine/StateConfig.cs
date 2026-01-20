using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.StateMachine
{
    public sealed class StateConfig<TTrigger> where TTrigger : Enum
    {
        private readonly Dictionary<TTrigger, IState> m_transitions = new();

        private Func<StatePayload, Task> m_onEnter;
        private Func<Task> m_onExit;

        public StateConfig<TTrigger> Permit(TTrigger trigger, IState target)
        {
            m_transitions[trigger] = target;
            return this;
        }

        public StateConfig<TTrigger> OnEntryAsync(Func<StatePayload, Task> handler)
        {
            m_onEnter = handler;
            return this;
        }

        public StateConfig<TTrigger> OnExitAsync(Func<Task> handler)
        {
            m_onExit = handler;
            return this;
        }

        internal bool TryGetTransition(TTrigger trigger, out IState target) => m_transitions.TryGetValue(trigger, out target);

        internal Task EnterAsync(StatePayload payload) => m_onEnter?.Invoke(payload) ?? Task.CompletedTask;

        internal Task ExitAsync() => m_onExit?.Invoke() ?? Task.CompletedTask;
    }
}
