using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachines
{
    public sealed class StateMachine<TState, TTrigger>
        where TState : Enum
        where TTrigger : Enum
    {
        private readonly Dictionary<TState, StateConfig<TState, TTrigger>> m_configs = new();
        private TState m_currentState;

        public TState CurrentState => m_currentState;

        public StateMachine(TState initialState) => m_currentState = initialState;

        public StateConfig<TState, TTrigger> Configure(TState state)
        {
            if (!m_configs.TryGetValue(state, out StateConfig<TState, TTrigger> config))
            {
                config = new StateConfig<TState, TTrigger>();
                m_configs[state] = config;
            }

            return config;
        }

        public async Task FireAsync(TTrigger trigger, StatePayload payload = null)
        {
            var config = m_configs[m_currentState];

            if (!config.TryGetTransition(trigger, out var transition))
            {
                Debug.LogError($"Invalid trigger {trigger} from {m_currentState}");
                return;
            }

            if (!EqualityComparer<TState>.Default.Equals(m_currentState, transition))
            {
                await config.ExitAsync();
                m_currentState = transition;
                await m_configs[m_currentState].EnterAsync(payload);
            }
        }
    }
}