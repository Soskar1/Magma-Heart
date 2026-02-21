using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Selection
{
    public class AttackSelector : AbilitySelector
    {
        private readonly AbilityDefinition m_attack;

        public AttackSelector(AbilityDefinition ability) => m_attack = ability;

        protected override AbilityPlan TrySelectAbility(GameWorld world, EntityModel executor, Vector3 worldPosition)
        {
            if (world.TryGetEntityAtPosition(worldPosition, out Entity target))
            {
                AbilityEngine engine = new AbilityEngine();
                AbilityPlan plan = engine.Plan(world, executor.Id, m_attack, AbilityTarget.EntityTarget(target.Model.Id, worldPosition));

                if (plan.IsLegal)
                    return plan;
            }

            return null;
        }
    }
}
