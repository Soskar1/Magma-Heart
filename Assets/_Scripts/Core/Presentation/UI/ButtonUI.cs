using System;
using MagmaHeart.Core.Input.Mouse;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Presentation.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(UIMouseInteraction))]
    public class ButtonUI : MonoBehaviour
    {
        [SerializeField] private Sprite m_defaultSprite;
        [SerializeField] private Sprite m_mouseHoverSprite;
        [SerializeField] private Sprite m_pressedSprite;
        private Image m_image;
        private UIMouseInteraction m_interaction;

        private void Start() => m_image = GetComponent<Image>();

        private void OnEnable()
        {
            if (m_interaction == null)
                m_interaction = GetComponent<UIMouseInteraction>();

            m_interaction.OnPointerEnterEvent += OnPointerEnter;
            m_interaction.OnPointerExitEvent += OnPointerExit;
            m_interaction.OnPointerDownEvent += OnPointerDown;
        }

        private void OnDisable()
        {
            m_image.sprite = m_defaultSprite;

            m_interaction.OnPointerEnterEvent -= OnPointerEnter;
            m_interaction.OnPointerExitEvent -= OnPointerExit;
            m_interaction.OnPointerDownEvent -= OnPointerDown;
        }

        public void OnPointerEnter(object _, EventArgs __) => m_image.sprite = m_mouseHoverSprite;
        public void OnPointerExit(object _, EventArgs __) => m_image.sprite = m_defaultSprite;
        public void OnPointerDown(object _, EventArgs __) => m_image.sprite = m_pressedSprite;
    }
}