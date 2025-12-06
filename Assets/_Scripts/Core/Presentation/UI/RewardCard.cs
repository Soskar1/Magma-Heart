using MagmaHeart.Core.Artifacts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagmaHeart.Core.Presentation.UI
{
    public class RewardCard : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI m_nameText;
        [SerializeField] private TextMeshProUGUI m_rarityText;
        [SerializeField] private TextMeshProUGUI m_descriptionText;
        [SerializeField] private Image m_image;
        private ArtifactData m_currentArtifactData;

        public event EventHandler<OnCardClickedArgs> OnArtifactDataPicked;

        public void Display(ArtifactData data)
        {
            m_currentArtifactData = data;

            m_nameText.text = data.Name;
            m_rarityText.text = data.Rarity.ToString();
            m_descriptionText.text = data.Description;
            m_image.sprite = data.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnCardClickedArgs args = new OnCardClickedArgs(m_currentArtifactData);
                OnArtifactDataPicked?.Invoke(this, args);
            }
        }
    }
}

