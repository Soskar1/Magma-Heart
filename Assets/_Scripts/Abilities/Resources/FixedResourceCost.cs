using MagmaHeart.Abilities.Targeting;
using System;
using UnityEngine;

namespace MagmaHeart.Abilities.Resources
{
    [Serializable]
    public class FixedResourceCost : CostModule
    {
        [SerializeField] private ParameterId m_parameterId;
        [SerializeField] private int m_amount;

        public override ResourceCost ComputeCost(IGameWorld world, int executorId, AbilityTarget target)
        {
            var cost = new ResourceCost();
            
            if (m_parameterId != null && m_amount != 0)
                cost.Add(m_parameterId, m_amount);
            
            return cost;
        }
    }
}
