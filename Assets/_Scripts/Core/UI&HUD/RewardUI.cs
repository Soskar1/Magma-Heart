using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.StateMachines;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class RewardUI : MonoBehaviour, IDisplayable, IRewardStateListener
    {
        [SerializeField] private GameObject m_visual;
        [SerializeField] private Button m_getRewardButton;
        [SerializeField] private List<RewardCard> m_rewardCards;
        private GameStateMachine m_stateMachine;
        private ArtifactData m_currentlyPickedArtifact;

        public void Initialize(GameStateMachine stateMachine)
        {
            m_stateMachine = stateMachine;
            
            foreach (RewardCard card in m_rewardCards)
                card.OnArtifactDataPicked += HandleOnArtifactPicked;
        }

        public void OnDisable()
        {
            foreach (RewardCard card in m_rewardCards)
                card.OnArtifactDataPicked -= HandleOnArtifactPicked;
        }

        public void Show() => m_visual.SetActive(true);
        public void Hide() => m_visual.SetActive(false);

        public void GetReward()
        {
            m_stateMachine.ChangeState(StateMachineStates.Action);
        }

        public void EnterRewardState() { }
        public void ExitRewardState()
        {
            Hide();
            m_getRewardButton.interactable = false;
        }

        public void HandleOnBattleRewardCalculated(object obj, OnBattleRewardCalculatedArgs args)
        {
            for (int i = 0; i < args.Rewards.Count; ++i)
                m_rewardCards[i].Display(args.Rewards[i]);

            Show();
        }

        public void HandleOnArtifactPicked(object obj, OnCardClickedArgs args)
        {
            m_getRewardButton.interactable = true;
            m_currentlyPickedArtifact = args.ClickedArtifactData;
        }
    }
}