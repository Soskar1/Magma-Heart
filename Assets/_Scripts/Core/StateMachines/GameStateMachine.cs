using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Presentation.UI;
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
        
        public GameStateMachine(ActionState action, CombatState combat, RewardState reward)
        {
            m_states = new Dictionary<StateMachineStates, IState>()
            {
                { StateMachineStates.Action, action },
                { StateMachineStates.Combat, combat },
                { StateMachineStates.Reward, reward },
            };

            m_validStateSwitch = new Dictionary<StateMachineStates, StateMachineStates>()
            {
                { StateMachineStates.Action, StateMachineStates.Combat },
                { StateMachineStates.Combat, StateMachineStates.Reward },
                { StateMachineStates.Reward, StateMachineStates.Action }
            };

            m_stateMachine = new StateMachine(action);
            m_currentState = StateMachineStates.Action;
            m_stateMachine.ChangeState(action);
        }

        private void ChangeState(StateMachineStates state)
        {
            if (state != m_currentState && m_validStateSwitch[m_currentState] == state)
            {
                m_stateMachine.ChangeState(m_states[state]);
                m_currentState = state;
            }
            else
            {
                Debug.LogWarning($"Can't switch from {m_currentState} state to {state} state");
            }
        }

        public void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args) => ChangeState(StateMachineStates.Combat);
        
        public void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs args)
        {
            if (args.IsPlayerVictory)
                ChangeState(StateMachineStates.Reward);
        }

        public void HandleOnRewardPicked(object obj, OnRewardPickedArgs args) => ChangeState(StateMachineStates.Action);
    }
}