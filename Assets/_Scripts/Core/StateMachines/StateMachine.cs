using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.StateMachines
{
    public sealed class StateMachine<TTrigger>
        where TTrigger : Enum
    {
        private readonly Dictionary<IState, StateConfig<TTrigger>> m_configs = new();
        private IState m_currentState;

        public StateMachine(IState initialState) => m_currentState = initialState;

        public StateConfig<TTrigger> Configure(IState state)
        {
            if (!m_configs.TryGetValue(state, out StateConfig<TTrigger> config))
            {
                config = new StateConfig<TTrigger>();
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

            if (!ReferenceEquals(m_currentState, transition))
            {
                await config.ExitAsync();
                m_currentState = transition;
                await m_configs[m_currentState].EnterAsync(payload);
            }
        }
    }
}