using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagmaHeart.Core.Input.Mouse
{
    public class UIMouseInteraction : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler OnPointerDownEvent;
        public event EventHandler OnPointerEnterEvent;
        public event EventHandler OnPointerExitEvent;

        public void OnPointerDown(PointerEventData _) => OnPointerDownEvent?.Invoke(this, EventArgs.Empty);
        public void OnPointerEnter(PointerEventData _) => OnPointerEnterEvent?.Invoke(this, EventArgs.Empty);
        public void OnPointerExit(PointerEventData _) => OnPointerExitEvent?.Invoke(this, EventArgs.Empty);
    }
}
