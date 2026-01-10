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
        [SerializeField] private Image m_backgroundImage;

        [SerializeField] private Sprite m_defaultBackground;
        [SerializeField] private Sprite m_selectedBackground;
        [SerializeField] private Sprite m_mouseHoverBackground;

        public ArtifactData ArtifactData { get; private set; }
        public bool IsSelected { get; private set; }

        public event EventHandler<OnCardClickedArgs> OnArtifactDataPicked;

        public void Display(ArtifactData data)
        {
            ArtifactData = data;

            m_nameText.text = data.Name;
            m_rarityText.text = data.Rarity.ToString();
            m_descriptionText.text = data.Description;
            m_image.sprite = data.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Select();

                OnCardClickedArgs args = new OnCardClickedArgs(this);
                OnArtifactDataPicked?.Invoke(this, args);
            }
        }

        private void Select()
        {
            IsSelected = true;
            m_backgroundImage.sprite = m_selectedBackground;
        }

        public void Unselect()
        {
            IsSelected = false;
            m_backgroundImage.sprite = m_defaultBackground;
        }

        public void UseDefaultBackground() => m_backgroundImage.sprite = m_defaultBackground;
        public void UseMouseHoverBackground() => m_backgroundImage.sprite = m_mouseHoverBackground;
    }
}

