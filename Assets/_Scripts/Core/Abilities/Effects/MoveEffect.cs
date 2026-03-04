using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record MoveEffect(int ExecutorId, List<Vector3> Path) : AbilityEffect(ExecutorId);

    [Serializable]
    public class BuildMoveEffect : EffectModule
    {
        public override IEnumerable<AbilityEffect> BuildEffects(IGameWorld world, int executorId, AbilityTarget target)
        {
            bool isTargetingPath = target.Kind.HasFlag(TargetKind.Path);

            if (!isTargetingPath || target.Path == null || target.Path.Count == 0)
                return new List<AbilityEffect>();

            return new List<AbilityEffect>()
            {
                new MoveEffect(executorId, target.Path)
            };
        }
    }
}