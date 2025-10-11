using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.StateMachines;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class RewardUI : MonoBehaviour, IDisplayable, IRewardStateListener
    {
        [SerializeField] private GameObject m_visual;
        [SerializeField] private List<RewardCard> m_rewardCards;
        private GameStateMachine m_stateMachine;

        public void Initialize(GameStateMachine stateMachine) => m_stateMachine = stateMachine;
        
        public void Show() => m_visual.SetActive(true);
        public void Hide() => m_visual.SetActive(false);

        public void GetReward()
        {
            m_stateMachine.ChangeState(StateMachineStates.Action);
        }

        public void EnterRewardState() { }
        public void ExitRewardState() => Hide();

        public void HandleOnBattleRewardCalculated(object obj, OnBattleRewardCalculatedArgs args)
        {
            for (int i = 0; i < args.Rewards.Count; ++i)
                m_rewardCards[i].Display(args.Rewards[i]);

            Show();
        }
    }
}