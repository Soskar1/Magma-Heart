using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class DamageEffectPresenter : IEffectPresenter<DamageEffect>
    {
        private readonly EntityOutlinePresenter m_outlinePresenter;
        private readonly CursorPresenter m_cursorPresenter;
        private readonly GameWorld m_world;
        private Entity m_currentOutlinedEntity;

        public DamageEffectPresenter(GameWorld world, EntityOutlinePresenter outlinePresenter, CursorPresenter cursorPresenter)
        {
            m_world = world;
            m_outlinePresenter = outlinePresenter;
            m_cursorPresenter = cursorPresenter;
        }

        public void Present(DamageEffect effect)
        {
            if (!m_world.TryGetEntity(effect.TargetId, out Entity entity))
            {
                Debug.LogWarning($"[{nameof(DamageEffectPresenter)}]: Can't get entity with id {effect.TargetId}.");
                return;
            }

            m_currentOutlinedEntity = entity;
            m_outlinePresenter.OutlineEntity(entity, OutlineType.CanBeAttacked);
            m_cursorPresenter.SetCursor(CursorType.Attack);
        }

        public void Hide()
        {
            m_cursorPresenter.SetCursor(CursorType.Default);

            if (m_currentOutlinedEntity == null)
                return;

            m_outlinePresenter.ClearOutline(m_currentOutlinedEntity);
            m_currentOutlinedEntity = null;
        }
    }
}
