using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.Input.Mouse
{
    public class ActionHoverHandler : IHoverHandler
    {
        private Entity m_currentEntity;

        public void ClearHover()
        {
            if (m_currentEntity != null)
                m_currentEntity?.Outline.RemoveOutline();

            m_currentEntity = null;
        }
        public void Visit(RaycastHoverResult result)
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
                if (m_currentEntity != null)
                    m_currentEntity.Outline.RemoveOutline();

                m_currentEntity = result.Entity;

                if (m_currentEntity.Model.IsPlayer)
                    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ALLY_OUTLINE);
                else
                    m_currentEntity.Outline.ApplyOutline(OutlineSettings.ENEMY_OUTLINE);
            }
        }

        public void Visit(UIHoverResult result) { }

        public void Visit(CombatHoverResult result) { }
    }
}