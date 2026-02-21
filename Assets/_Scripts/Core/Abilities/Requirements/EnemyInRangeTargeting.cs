using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Requirements;
using MagmaHeart.Abilities.Targeting;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Requirements
{
    [System.Serializable]
    public class EnemyInRangeTargeting : IAbilityRequirement
    {
        [SerializeField] private int m_minRange;
        [SerializeField] private int m_maxRange;

        public bool IsMet(IGameWorld world, int executorId, AbilityTarget target)
        {
            bool isTargetingEntity = target.Kind.HasFlag(TargetKind.Entity);
            
            if (!isTargetingEntity)
                return false;

            int distance = world.GetDistance(executorId, target.EntityId);

            if (distance < m_minRange || distance > m_maxRange)
                return false;

            return true;
        }
    }
}
