using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;
using MagmaHeart.AI;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Input.Mouse;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Selection
{
    public class AbilitySelector
    {
        private readonly AbilityEngine m_abilityEngine;
        private readonly IBoardGameWorld m_world;

        public AbilitySelector(IBoardGameWorld world)
        {
            m_abilityEngine = new AbilityEngine();
            m_world = world;
        }

        public AbilityPlan SelectAbility(HoverResult hoverResult, EntityModel executor, AbilityDefinition armedAbility)
        {
            if (hoverResult == null || executor == null)
                return null;

            if (hoverResult.Type.HasFlag(HoverResultType.UI))
                return null;

            if (armedAbility != null)
                return SelectExplicitAbility(hoverResult, executor, armedAbility);

            return SelectDefaultAbility(hoverResult, executor);
        }

        private AbilityPlan SelectDefaultAbility(HoverResult hoverResult, EntityModel executor)
        {
            bool hoversEntity = hoverResult.Type.HasFlag(HoverResultType.Entity);
            bool hoversTile = hoverResult.Type.HasFlag(HoverResultType.Tile);

            if (hoversEntity)
            {
                Vector3 executorPosition = m_world.GetEntityPosition(executor.Id);
                Vector3 targetTilePosition = m_world.GetEntityPosition(hoverResult.Entity.Model.Id);

                if (executorPosition != targetTilePosition && PathFinder.TryFindPathToEntity(m_world, executorPosition, targetTilePosition, out List<Vector3> path))
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

        private AbilityPlan SelectExplicitAbility(HoverResult hoverResult, EntityModel executor, AbilityDefinition ability)
        {
            AbilityTarget target = BuildTargetForAbility(hoverResult, executor, ability);

            if (target == null)
                return null;

            AbilityPlan plan = m_abilityEngine.Plan(m_world, executor.Id, ability, target);
            return plan.IsLegal ? plan : null;
        }

        private AbilityTarget BuildTargetForAbility(HoverResult hoverResult, EntityModel executor, AbilityDefinition ability)
        {
            if (ability.TargetKind.HasFlag(TargetKind.Self))
                return AbilityTarget.EntityTarget(executor.Id, null);

            bool hoversEntity = hoverResult.Type.HasFlag(HoverResultType.Entity);
            bool hoversTile = hoverResult.Type.HasFlag(HoverResultType.Tile);

            AbilityTarget currentTarget = null;

            var abilityNeedsPathToEntity = ability.TargetKind.HasFlag(TargetKind.Entity) && ability.TargetKind.HasFlag(TargetKind.Path);
            if (hoversEntity && executor.Id != hoverResult.Entity.Model.Id && abilityNeedsPathToEntity)
            {
                Vector3 executorPosition = m_world.GetEntityPosition(executor.Id);
                Vector3 targetTilePosition = hoverResult.Tile.Position.ToVector3();

                if (PathFinder.TryFindPathToEntity(m_world, executorPosition, targetTilePosition, out List<Vector3> path))
                    currentTarget = AbilityTarget.EntityTarget(hoverResult.Entity.Model.Id, path);

                return currentTarget;
            }

            if (ability.TargetKind.HasFlag(TargetKind.Path) && hoversTile)
            {
                Vector3 from = m_world.GetEntityPosition(executor.Id);
                Vector3 to = hoverResult.Tile.Position.ToVector3();

                if (m_world.TryFindPath(from, to, out List<Vector3> path) && path != null)
                {
                    if (currentTarget != null)
                    {
                        currentTarget = currentTarget with
                        {
                            Kind = currentTarget.Kind | TargetKind.Path,
                            Path = path
                        };
                    }
                    else
                    {
                        currentTarget = AbilityTarget.PathTarget(path);
                    }
                }
            }

            return currentTarget;
        }
    }
}
