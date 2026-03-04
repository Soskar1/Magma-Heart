using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class DamageEffectPresenter : IEffectPresenter<DamageEffect>
    {
        private readonly EntityOutlinePresenter m_outlinePresenter;

        public DamageEffectPresenter(EntityOutlinePresenter outlinePresenter)
        {
            m_outlinePresenter = outlinePresenter;
        }

        public void Present(GameWorld world, DamageEffect effect)
        {
            if (!world.TryGetEntity(effect.TargetId, out Entity entity))
            {
                Debug.LogWarning($"[{nameof(DamageEffectPresenter)}]: Can't get entity with id {effect.TargetId}.");
                return;
            }

            m_outlinePresenter.OutlineEntity(entity, OutlineType.CanBeAttacked);
        }
    }
}
