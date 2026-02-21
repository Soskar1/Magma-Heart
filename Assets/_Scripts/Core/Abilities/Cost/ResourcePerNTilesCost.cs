using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Cost
{
    [Serializable]
    public class ResourcePerNTilesCost : CostModule
    {
        [SerializeField] private ResourceId m_resourceId;
        [SerializeField] private int m_tilesPerResource;

        public override ResourceCost ComputeCost(IGameWorld world, int executorId, AbilityTarget target)
        {
            if (m_resourceId == null || m_tilesPerResource <= 0)
                return ResourceCost.Zero;

            int steps = 0;

            if (target.Kind == TargetKind.Path && target.Path != null && target.Path.Count > 0)
            {
                steps = Math.Max(0, target.Path.Count - 1);
            }
            else if (target.Kind == TargetKind.Position)
            {
                Vector3 from = world.GetEntityPosition(executorId);
                bool foundPath = world.TryFindPath(from, target.Position, out List<Vector3> path);
                if (foundPath && path == null)
                    return ResourceCost.Zero;
                
                steps = Math.Max(0, path.Count - 1);
            }
            else
            {
                return ResourceCost.Zero;
            }

            int amount = Mathf.CeilToInt(steps / (float)m_tilesPerResource);
            ResourceCost cost = new ResourceCost();
            cost.Add(m_resourceId, amount);
            
            return cost;
        }
    }
}
