using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Input.Mouse;
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
        private ArtifactData m_currentlyPickedArtifact;
        private Inventory m_inventory;

        public event EventHandler<OnRewardPickedEventArgs> OnRewardPicked;
        
        [SerializeField] private Sprite m_defaultRewardCard;
        [SerializeField] private Sprite m_mouseHoverRewardCard;
        [SerializeField] private Sprite m_selectedRewardCard;

        private Dictionary<UIMouseInteraction, RewardCard> m_currentRewardCards = new Dictionary<UIMouseInteraction, RewardCard>();

        public void Initialize(Inventory inventory)
        {
            m_inventory = inventory;
        }

        public void Show() => m_visual.SetActive(true);
        public void Hide()
        {
            foreach (var keyValuePair in m_currentRewardCards)
            {
                UIMouseInteraction uiMouseInteraction = keyValuePair.Key;
                RewardCard card = keyValuePair.Value;

                card.OnArtifactDataPicked -= HandleOnArtifactPicked;
                uiMouseInteraction.OnPointerEnterEvent -= OnPointerEnterCard;
                uiMouseInteraction.OnPointerExitEvent -= OnPointerExitCard;

                GameObject.Destroy(card.gameObject); // TODO: chanzge to object pool
            }

            m_visual.SetActive(false);
            m_currentRewardCards.Clear();

            m_getRewardButton.interactable = false;
            m_getRewardButton.GetComponent<ButtonUI>().enabled = false;
        }

        public void GetReward()
        {
            Hide();

            m_inventory.TryPick(m_currentlyPickedArtifact);

            OnRewardPickedEventArgs args = new OnRewardPickedEventArgs(m_currentlyPickedArtifact);
            OnRewardPicked?.Invoke(this, args);
        }

        public void DisplayRewards(IEnumerable<ArtifactData> rewards)
        {
            foreach (ArtifactData reward in rewards)
            {
                RewardCard card = Instantiate(m_rewardCardPrefab, m_rewardCardParent);
                card.OnArtifactDataPicked += HandleOnArtifactPicked;

                UIMouseInteraction uiMouseInteraction = card.GetComponent<UIMouseInteraction>();
                uiMouseInteraction.OnPointerEnterEvent += OnPointerEnterCard;
                uiMouseInteraction.OnPointerExitEvent += OnPointerExitCard;

                m_currentRewardCards.Add(uiMouseInteraction, card);
                card.Display(reward);
            }

            Show();
        }

        public void HandleOnArtifactPicked(object obj, OnCardClickedArgs args)
        {
            m_getRewardButton.interactable = true;
            m_getRewardButton.GetComponent<ButtonUI>().enabled = true;
            m_currentlyPickedArtifact = args.RewardCard.ArtifactData;

            foreach (RewardCard rewardCard in m_currentRewardCards.Values)
                if (rewardCard != args.RewardCard && rewardCard.IsSelected)
                    rewardCard.Unselect();
        }

        private void OnPointerEnterCard(object obj, EventArgs _)
        {
            UIMouseInteraction mouseInteraction = (UIMouseInteraction)obj;
            RewardCard hoveredRewardCard = m_currentRewardCards[mouseInteraction];

            if (hoveredRewardCard.IsSelected)
                return;

            hoveredRewardCard.UseMouseHoverBackground();
        }

        private void OnPointerExitCard(object obj, EventArgs _)
        {
            UIMouseInteraction mouseInteraction = (UIMouseInteraction)obj;
            RewardCard rewardCard = m_currentRewardCards[mouseInteraction];

            if (rewardCard.IsSelected)
                return;

            rewardCard.UseDefaultBackground();
        }
    }
}