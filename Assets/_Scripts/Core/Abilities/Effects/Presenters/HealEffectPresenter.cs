using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class HealEffectPresenter : IEffectPresenter<HealEffect>
    {
        private readonly EntityOutlinePresenter m_outlinePresenter;
        private readonly GameWorld m_world;
        private Entity m_currentOutlinedEntity;

        public HealEffectPresenter(GameWorld world, EntityOutlinePresenter outlinePresenter)
        {
            m_world = world;
            m_outlinePresenter = outlinePresenter;
        }

        public void Present(HealEffect effect)
        {
            if (!m_world.TryGetEntity(effect.ExecutorId, out Entity entity))
            {
                Debug.LogWarning($"[{nameof(HealEffectPresenter)}]: Can't get entity with id {effect.ExecutorId}.");
                return;
            }

            m_currentOutlinedEntity = entity;
            m_outlinePresenter.OutlineEntity(entity, OutlineType.Ally);
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
