using MagmaHeart.Core.CombatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.StateMachines
{
    public class GameStateMachine
    {
        private readonly StateMachine m_stateMachine;
        private readonly Dictionary<StateMachineStates, IState> m_states;
        private readonly Dictionary<StateMachineStates, StateMachineStates> m_validStateSwitch;

        private StateMachineStates m_currentState;
        
        public GameStateMachine(ActionState action, CombatState combat, Battle battle)
        {
            m_states = new Dictionary<StateMachineStates, IState>()
            {
                { StateMachineStates.Action, action },
                { StateMachineStates.Combat, combat },
            };

            m_validStateSwitch = new Dictionary<StateMachineStates, StateMachineStates>()
            {
                { StateMachineStates.Action, StateMachineStates.Combat },
                { StateMachineStates.Combat, StateMachineStates.Action }
            };

            m_stateMachine = new StateMachine(action);
            m_currentState = StateMachineStates.Action;

            battle.OnPlayerVictory += HandleOnPlayerVictory;
        }

        public void ChangeState(StateMachineStates state, params object[] args)
        {
            if (state != m_currentState && m_validStateSwitch[m_currentState] == state)
            {
                m_stateMachine.ChangeState(m_states[state], args);
                m_currentState = state;
            }
            else
            {
                Debug.LogWarning($"Can't switch from {m_currentState} state to {state} state");
            }
        }

        private void HandleOnPlayerVictory(object obj, EventArgs args) => ChangeState(StateMachineStates.Action, args);
    }
}