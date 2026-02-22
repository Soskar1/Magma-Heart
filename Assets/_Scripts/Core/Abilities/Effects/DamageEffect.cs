using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record DamageEffect(int ExecutorId, int TargetId, int Damage) : AbilityEffect(ExecutorId);

    [Serializable]
    public class BuildDamageEffect : EffectModule
    {
        [SerializeField] private int m_initialDamage = 5;

        public override IEnumerable<AbilityEffect> BuildEffects(IGameWorld world, int executorId, AbilityTarget target)
        {
            bool isTargetingEntity = target.Kind.HasFlag(TargetKind.Entity);
            
            if (!isTargetingEntity)
                return new List<AbilityEffect>();

            return new List<AbilityEffect>()
            {
                new DamageEffect(executorId, target.EntityId, m_initialDamage)
            };
        }
    }
}
