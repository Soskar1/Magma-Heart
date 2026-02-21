using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Selection
{
    public class MovementSelector : AbilitySelector
    {
        private readonly AbilityDefinition m_movement;

        public MovementSelector(AbilityDefinition ability) => m_movement = ability;

        protected override AbilityPlan TrySelectAbility(GameWorld world, EntityModel executor, Vector3 worldPosition)
        {
            AbilityEngine engine = new AbilityEngine();
            AbilityPlan plan = engine.Plan(world, executor.Id, m_movement, AbilityTarget.PositionTarget(worldPosition));

            if (plan.IsLegal)
                return plan;

            return null;
        }
    }
}
