using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Input.Mouse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                Vector3 executorPosition = m_world.GetEntityPosition(executor.Id);
                Vector3 targetTilePosition = m_world.GetEntityPosition(hoverResult.Entity.Model.Id);

                if (executorPosition != targetTilePosition && TryFindPathToEntity(executorPosition, targetTilePosition, out List<Vector3> path))
                {
                    AbilityTarget target = AbilityTarget.EntityTarget(hoverResult.Entity.Model.Id, path);
                    AbilityPlan plan = m_abilityEngine.Plan(m_world, executor.Id, executor.AttackAbility, target);

                    if (plan.IsLegal)
                        return plan;
                }
            }

            if (hoversTile)
            {
                Vector3 from = m_world.GetEntityPosition(executor.Id);
                Vector3 to = hoverResult.Tile.Position.ToVector3();
                bool foundValidPath = m_world.TryFindPath(from, to, out List<Vector3> path);

                if (!foundValidPath)
                    return null;

                AbilityTarget target = AbilityTarget.PathTarget(path);
                AbilityPlan plan = m_abilityEngine.Plan(m_world, executor.Id, executor.MovementAbility, target);

                if (plan.IsLegal)
                    return plan;
            }

            return null;
        }

        private bool TryFindPathToEntity(Vector3 executorPosition, Vector3 targetTilePosition, out List<Vector3> path)
        {
            IEnumerable<Vector3> sortedCandidates = new[]
            {
                targetTilePosition + Vector3.up,
                targetTilePosition + Vector3.down,
                targetTilePosition + Vector3.left,
                targetTilePosition + Vector3.right,
            }.OrderBy(candidate => Vector3.SqrMagnitude(executorPosition - (Vector3)candidate));

            foreach (Vector3 candidate in sortedCandidates)
            {
                if (m_world.TryFindPath(executorPosition, candidate, out path))
                    return true;
            }

            path = null;
            return false;
        }
    }
}
