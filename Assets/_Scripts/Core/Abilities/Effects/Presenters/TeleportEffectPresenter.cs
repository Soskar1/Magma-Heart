using MagmaHeart.Core.Abilities.Presentation;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public class TeleportEffectPresenter : IEffectPresenter<TeleportEffect>
    {
        private readonly CombatTilemapPresenter m_combatTilemapPresenter;
        private readonly GameWorld m_world;
        private readonly EntityOutlinePresenter m_outlinePresenter;
        private Entity m_currentOutlinedEntity;

        public TeleportEffectPresenter(CombatTilemapPresenter combatTilemapPresenter, GameWorld world, EntityOutlinePresenter outlinePresenter)
        {
            m_combatTilemapPresenter = combatTilemapPresenter;
            m_world = world;
            m_outlinePresenter = outlinePresenter;
        }

        public void Present(TeleportEffect effect)
        {
            m_combatTilemapPresenter.DisplayTile(effect.TeleportPosition.ToVector3Int(), true);

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
            m_combatTilemapPresenter.Clear();

            if (m_currentOutlinedEntity == null)
                return;

            m_outlinePresenter.ClearOutline(m_currentOutlinedEntity);
            m_currentOutlinedEntity = null;
        }
    }
}
