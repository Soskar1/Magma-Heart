using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class DamageEffectPresenter : IEffectPresenter<DamageEffect>
    {
        private readonly EntityOutlinePresenter m_outlinePresenter;
        private readonly GameWorld m_world;
        private Entity m_currentOutlinedEntity;

        public DamageEffectPresenter(GameWorld world, EntityOutlinePresenter outlinePresenter)
        {
            m_world = world;
            m_outlinePresenter = outlinePresenter;
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
        }

        public void Hide()
        {
            if (m_currentOutlinedEntity == null)
                return;

            m_outlinePresenter.ClearOutline(m_currentOutlinedEntity);
            m_currentOutlinedEntity = null;
        }
    }
}
