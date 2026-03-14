using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record TeleportEffect(int ExecutorId, Vector3 TeleportPosition) : AbilityEffect(ExecutorId);

    [Serializable]
    public class BuildTeleportEffect : EffectModule
    {
        public override IEnumerable<AbilityEffect> BuildEffects(IGameWorld world, int executorId, AbilityTarget target)
        {
            bool isTargetingPath = target.Kind.HasFlag(TargetKind.Path);

            if (!isTargetingPath || target.Path == null || target.Path.Count == 0)
                return new List<AbilityEffect>();

            return new List<AbilityEffect>()
            {
                new TeleportEffect(executorId, target.Path.Last())
            };
        }
    }
}