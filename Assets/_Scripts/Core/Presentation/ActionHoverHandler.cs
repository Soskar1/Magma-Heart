using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class ActionHoverHandler : IHoverHandler
    {
        private IHoverable m_current;

        public void HandleHover(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, Mathf.Infinity);

            if (hit.collider != null && hit.collider.TryGetComponent(out IHoverable h))
            {
                if (m_current != h)
                {
                    m_current?.UndoHover();
                    m_current = h;
                    m_current.ApplyHover();
                }
            }
            else
            {
                m_current?.UndoHover();
                m_current = null;
            }
        }
    }
}