using MagmaHeart.Core;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Abilities.Effects.Presenters;
using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace Assets._Scripts.Core.Abilities.Effects.Presenters
{
    public class KnockbackEffectPresenter : IEffectPresenter<KnockbackEffect>
    {
        private readonly EntityOutlinePresenter m_outlinePresenter;
        private readonly CombatTilemapPresenter m_combatTilemapPresenter;
        private readonly GameWorld m_world;
        private Entity m_currentOutlinedEntity;

        public KnockbackEffectPresenter(GameWorld world, EntityOutlinePresenter outlinePresenter, CombatTilemapPresenter combatTilemapPresenter)
        {
            m_outlinePresenter = outlinePresenter;
            m_combatTilemapPresenter = combatTilemapPresenter;
            m_world = world;
        }

        public void Present(KnockbackEffect effect)
        {
            if (!m_world.TryGetEntity(effect.TargetId, out Entity entity))
            {
                Debug.LogWarning($"[{nameof(KnockbackEffectPresenter)}]: Can't get entity with id {effect.TargetId}.");
                return;
            }

            m_combatTilemapPresenter.DisplayTile(effect.NewPosition.ToVector3Int(), true);

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
