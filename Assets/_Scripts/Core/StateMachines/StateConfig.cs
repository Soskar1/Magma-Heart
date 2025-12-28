using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public sealed class StateConfig<TState, TTrigger>
        where TState : Enum
        where TTrigger : Enum
    {
        private readonly Dictionary<TTrigger, TState> m_transitions = new();

        private Func<StatePayload, Task> m_onEnter;
        private Func<Task> m_onExit;

        public StateConfig<TState, TTrigger> Permit(TTrigger trigger, TState target)
        {
            m_transitions[trigger] = target;
            return this;
        }

        public StateConfig<TState, TTrigger> OnEntryAsync(Func<Task> handler)
        {
            m_onEnter = _ => handler();
            return this;
        }

        public StateConfig<TState, TTrigger> OnEntryAsync(Func<StatePayload, Task> handler)
        {
            m_onEnter = handler;
            return this;
        }

        public StateConfig<TState, TTrigger> OnExitAsync(Func<Task> handler)
        {
            m_onExit = handler;
            return this;
        }

        internal bool TryGetTransition(TTrigger trigger, out TState target)
            => m_transitions.TryGetValue(trigger, out target);

        internal Task EnterAsync(StatePayload payload) => m_onEnter?.Invoke(payload) ?? Task.CompletedTask;

        internal Task ExitAsync() => m_onExit?.Invoke() ?? Task.CompletedTask;
    }
}
