using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.CombatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Presentation.UI
{
    public class RewardUI : MonoBehaviour, IDisplayable
    {
        [SerializeField] private GameObject m_visual;
        [SerializeField] private Button m_getRewardButton;
        [SerializeField] private List<RewardCard> m_rewardCards;
        private ArtifactData m_currentlyPickedArtifact;
        private BattleReward m_battleReward;

        public event EventHandler<OnRewardPickedArgs> OnRewardPicked;

        public void Initialize(BattleReward battleReward)
        {
            m_battleReward = battleReward;

            foreach (RewardCard card in m_rewardCards)
                card.OnArtifactDataPicked += HandleOnArtifactPicked;

            m_battleReward.OnBattleRewardCalculated += HandleOnBattleRewardCalculated;
        }

        public void OnDisable()
        {
            foreach (RewardCard card in m_rewardCards)
                card.OnArtifactDataPicked -= HandleOnArtifactPicked;

            m_battleReward.OnBattleRewardCalculated -= HandleOnBattleRewardCalculated;
        }

        public void Show() => m_visual.SetActive(true);
        public void Hide() => m_visual.SetActive(false);

        public void GetReward()
        {
            Hide();

            OnRewardPickedArgs args = new OnRewardPickedArgs(m_currentlyPickedArtifact);
            OnRewardPicked?.Invoke(this, args);
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