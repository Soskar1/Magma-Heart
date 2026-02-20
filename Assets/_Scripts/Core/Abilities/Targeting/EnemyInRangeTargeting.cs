using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Targeting
{
    [CreateAssetMenu(menuName = "Abilities/Targeting/Enemy In Range Targeting")]
    public class EnemyInRangeTargeting : TargetingModule
    {
        [SerializeField] private int m_minRange;
        [SerializeField] private int m_maxRange;

        public override bool ValidateChosenTarget(IGameWorld world, int executorId, AbilityTarget target)
        {
            if (target.Kind != TargetKind.Entity)
                return false;

            if (!world.IsEnemy(executorId, target.EntityId))
                return false;

            int distance = world.GetDistance(executorId, target.EntityId);

            if (distance < m_minRange || distance > m_maxRange)
                return false;

            return true;
        }
    }
}
