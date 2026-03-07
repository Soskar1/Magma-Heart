using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record HealEffect(int ExecutorId, int HealAmount, ParameterId HealthId) : AbilityEffect(ExecutorId);

    [System.Serializable]
    public class BuildHealEffect : EffectModule
    {
        [SerializeField] private int m_healAmount = 5;
        [SerializeField] private ParameterDatabase m_database;

        public override IEnumerable<AbilityEffect> BuildEffects(IGameWorld gameWorld, int executorId, AbilityTarget target)
        {
            bool isTargetingEntity = target.Kind.HasFlag(TargetKind.Entity);

            if (!isTargetingEntity || target.EntityId != executorId)
                return new List<AbilityEffect>();

            return new List<AbilityEffect>()
            {
                new HealEffect(executorId, m_healAmount, m_database.Health)
            };
        }
    }
}
