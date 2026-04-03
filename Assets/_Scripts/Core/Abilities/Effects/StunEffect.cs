using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record StunEffect(int ExecutorId, int TargetId) : AbilityEffect(ExecutorId);

    [System.Serializable]
    public class BuildStunEffect : EffectModule
    {
        public override IEnumerable<AbilityEffect> BuildEffects(IGameWorld gameWorld, int executorId, AbilityTarget target)
        {
            bool isTargetingEntity = target.Kind.HasFlag(TargetKind.Entity);

            if (!isTargetingEntity || target.EntityId == executorId)
                return new List<AbilityEffect>();

            return new List<AbilityEffect>()
            {
                new StunEffect(executorId, target.EntityId)
            };
        }
    }
}
