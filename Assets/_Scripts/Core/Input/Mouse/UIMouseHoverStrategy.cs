using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagmaHeart.Core.Input.Mouse
{
    public class UIMouseHoverStrategy : MouseHoverStrategy
    {
        private readonly GraphicRaycaster m_uiRaycast;

        public UIMouseHoverStrategy(GraphicRaycaster uiRaycaster) => m_uiRaycast = uiRaycaster;

        protected override HoverResult TryHover(Vector2 worldPosition)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Camera.main.WorldToScreenPoint(worldPosition)
            };

            List<RaycastResult> results = new();
            m_uiRaycast.Raycast(eventData, results);

            if (results.Count == 0)
                return null;

            foreach (RaycastResult r in results)
            {
                UIMouseInteraction detection = r.gameObject.GetComponentInParent<UIMouseInteraction>();
                if (detection != null)
                    return new UIHoverResult(detection.gameObject);
            }

            return null;
        }
    }
}
