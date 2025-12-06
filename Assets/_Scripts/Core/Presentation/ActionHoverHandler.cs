using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Presentation
{
    public class ActionHoverHandler : IHoverHandler
    {
        private Entity m_currentEntity;

        public void HandleHoverResult(HoverResult result)
        {
            if (result.Entity == null)
            {
                if (m_currentEntity != null)
                {
                    m_currentEntity.Outline.RemoveOutline();
                    m_currentEntity = null;
                }

                return;
            }

            if (result.Entity != m_currentEntity)
            {
                m_currentEntity?.Outline.RemoveOutline();
                m_currentEntity = result.Entity;

                if (m_currentEntity.Model.IsPlayer)
                    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ALLY_OUTLINE);
                else
                    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ENEMY_OUTLINE);
            }
        }

        public void ClearHover()
        {
            if (m_currentEntity != null)
                m_currentEntity?.Outline.RemoveOutline();

            m_currentEntity = null;
        }
    }
}