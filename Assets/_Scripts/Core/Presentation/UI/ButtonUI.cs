using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagmaHeart.Core.Presentation.UI
{
    [RequireComponent(typeof(Image))]
    public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private Sprite m_defaultSprite;
        [SerializeField] private Sprite m_mouseHoverSprite;
        [SerializeField] private Sprite m_pressedSprite;
        private Image m_image;

        private void Start() => m_image = GetComponent<Image>();

        private void OnDisable() => m_image.sprite = m_defaultSprite;

        public void OnPointerEnter(PointerEventData eventData) => m_image.sprite = m_mouseHoverSprite;
        public void OnPointerExit(PointerEventData eventData) => m_image.sprite = m_defaultSprite;
        public void OnPointerDown(PointerEventData eventData) => m_image.sprite = m_pressedSprite;
    }
}