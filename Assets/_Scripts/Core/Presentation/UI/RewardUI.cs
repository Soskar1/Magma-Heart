using MagmaHeart.Core.Artifacts;
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
        [SerializeField] private RewardCard m_rewardCardPrefab;
        [SerializeField] private Transform m_rewardCardParent;
        private List<RewardCard> m_currentRewardCards = new List<RewardCard>();
        private ArtifactData m_currentlyPickedArtifact;

        public event EventHandler<OnRewardPickedArgs> OnRewardPicked;

        public void Show() => m_visual.SetActive(true);
        public void Hide()
        {
            foreach (RewardCard card in m_currentRewardCards)
            {
                card.OnArtifactDataPicked -= HandleOnArtifactPicked;
                GameObject.Destroy(card.gameObject); // TODO: change to object pool
            }

            m_visual.SetActive(false);

            m_currentRewardCards.Clear();
        }

        public void GetReward()
        {
            Hide();

            OnRewardPickedArgs args = new OnRewardPickedArgs(m_currentlyPickedArtifact);
            OnRewardPicked?.Invoke(this, args);
        }

        public void DisplayRewards(IEnumerable<ArtifactData> rewards)
        {
            foreach (ArtifactData reward in rewards)
            {
                RewardCard card = Instantiate(m_rewardCardPrefab, m_rewardCardParent);
                card.OnArtifactDataPicked += HandleOnArtifactPicked;
                m_currentRewardCards.Add(card);

                card.Display(reward);
            }

            Show();
        }

        public void HandleOnArtifactPicked(object obj, OnCardClickedArgs args)
        {
            m_getRewardButton.interactable = true;
            m_currentlyPickedArtifact = args.ClickedArtifactData;
        }
    }
}