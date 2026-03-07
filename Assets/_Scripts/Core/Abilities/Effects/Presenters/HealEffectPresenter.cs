using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class HealEffectPresenter : IEffectPresenter<HealEffect>
    {
        private readonly EntityOutlinePresenter m_outlinePresenter;

        public HealEffectPresenter(EntityOutlinePresenter outlinePresenter)
        {
            m_outlinePresenter = outlinePresenter;
        }

        public void Present(GameWorld world, HealEffect effect)
        {
            if (!world.TryGetEntity(effect.ExecutorId, out Entity entity))
            {
                Debug.LogWarning($"[{nameof(HealEffectPresenter)}]: Can't get entity with id {effect.ExecutorId}.");
                return;
            }

            m_outlinePresenter.OutlineEntity(entity, OutlineType.Ally);
        }
    }
}
