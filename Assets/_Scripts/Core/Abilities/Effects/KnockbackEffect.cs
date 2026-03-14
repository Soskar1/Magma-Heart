using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record KnockbackEffect(int ExecutorId, int TargetId, Vector3 NewPosition) : AbilityEffect(ExecutorId);

    [Serializable]
    public class BuildKnockbackEffect : EffectModule
    {
        [SerializeField] private int m_knockbackDistance = 1;

        public override IEnumerable<AbilityEffect> BuildEffects(IGameWorld world, int executorId, AbilityTarget target)
        {
            bool isTargetingEntity = target.Kind.HasFlag(TargetKind.Entity);

            if (!isTargetingEntity)
                return new List<AbilityEffect>();

            Vector3 executorPosition = world.GetEntityPosition(executorId);
            Vector3 targetPosition = world.GetEntityPosition(target.EntityId);
            Vector3 direction = (targetPosition - executorPosition).normalized;

            return new List<AbilityEffect>()
            {
                new KnockbackEffect(executorId, target.EntityId, targetPosition + direction * m_knockbackDistance)
            };
        }
    }
}
