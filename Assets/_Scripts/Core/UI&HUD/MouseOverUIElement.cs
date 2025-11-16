using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagmaHeart.Core.UI
{
    public class MouseOverUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool m_isMouseOver = false;

        public event EventHandler OnMouseEnterUIElement;
        public event EventHandler OnMouseExitUIElement;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnterUIElement?.Invoke(this, EventArgs.Empty);
            m_isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExitUIElement?.Invoke(this, EventArgs.Empty);
            m_isMouseOver = false;
        }

        private void OnDisable()
        {
            if (m_isMouseOver)
            {
                OnMouseExitUIElement?.Invoke(this, EventArgs.Empty);
                m_isMouseOver = false;
            }
        }
    }
}