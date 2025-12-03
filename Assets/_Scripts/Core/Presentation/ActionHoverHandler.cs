using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Presentation
{
    public class ActionHoverHandler : IHoverHandler
    {
        private Entity m_currentEntity;

        public void HandleHover(Vector2 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.back, Mathf.Infinity);

            if (hit.collider != null && hit.collider.TryGetComponent(out Entity entity))
            {
                if (m_currentEntity != entity)
                {
                    m_currentEntity?.Outline.RemoveOutline();
                    m_currentEntity = entity;

                    if (m_currentEntity.Model.IsPlayer)
                        m_currentEntity.Outline.ApplyOutline(OutlineSettings.ALLY_OUTLINE);
                    else
                        m_currentEntity.Outline.ApplyOutline(OutlineSettings.ENEMY_OUTLINE);
                }
            }
            else
            {
                m_currentEntity?.Outline.RemoveOutline();
                m_currentEntity = null;
            }
        }
    }
}