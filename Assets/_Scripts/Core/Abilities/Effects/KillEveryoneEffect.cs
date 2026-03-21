using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.Abilities.Effects
{
    public record KillEveryoneEffect(int ExecutorId, IReadOnlyList<int> EntitiesToKill) : AbilityEffect(ExecutorId);

    [Serializable]
    public class BuildKillEveryoneEffect : EffectModule
    {
        public override IEnumerable<AbilityEffect> BuildEffects(IGameWorld world, int executorId, AbilityTarget target)
        {
            List<int> entitiesToKill = new List<int>();
            foreach (int entity in world.GetAllEntities())
            {
                if (entity != executorId)
                    entitiesToKill.Add(entity);
            }

            return new List<AbilityEffect>()
            {
                new KillEveryoneEffect(executorId, entitiesToKill)
            };
        }
    }
}
