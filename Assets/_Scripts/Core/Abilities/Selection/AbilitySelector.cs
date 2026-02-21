using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;
using UnityEngine;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Abilities.Selection
{
    public class AbilitySelector
    {
        private readonly AbilityEngine m_abilityEngine;
        private readonly IGameWorld m_world;

        public AbilitySelector(IGameWorld world)
        {
            m_abilityEngine = new AbilityEngine();
            m_world = world;
        }

        public AbilityPlan SelectAbility(HoverResult hoverResult, EntityModel executor)
        {
            if (hoverResult == null)
                return null;
           
            bool hoversUI = hoverResult.Type.HasFlag(HoverResultType.UI);
            bool hoversEntity = hoverResult.Type.HasFlag(HoverResultType.Entity);
            bool hoversTile = hoverResult.Type.HasFlag(HoverResultType.Tile);

            if (hoversUI)
                return null;

            if (hoversEntity)
            {
                AbilityTarget target = AbilityTarget.EntityTarget(hoverResult.Entity.Model.Id, hoverResult.Entity.Model.TilePosition);
                AbilityPlan plan = m_abilityEngine.Plan(m_world, executor.Id, executor.AttackAbility, target);

                if (plan.IsLegal)
                    return plan;
            }

            //if (hoversTile)
            //{
            //    Vector3 from = m_world.GetEntityPosition(executor.Id);
            //    Vector3 to = hoverResult.Tile.Position.ToVector3();
            //    bool foundValidPath = m_world.TryFindPath(from, to, out List<Vector3> path);

            //    if (!foundValidPath)
            //        return null;

            //    AbilityTarget target = AbilityTarget.PathTarget(path);
            //    AbilityPlan plan = m_abilityEngine.Plan(m_world, executor.Id, executor.MovementAbility, target);
                
            //    if (plan.IsLegal)
            //        return plan;
            //}

            return null;
        }
    }
}
