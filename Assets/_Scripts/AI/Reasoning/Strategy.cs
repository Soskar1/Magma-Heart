using MagmaHeart.AI.Reasoning.Plans;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning
{
    public abstract class Strategy : ScriptableObject
    {
        [SerializeField] private List<PlanDefinition> m_plans;
        public IReadOnlyList<PlanDefinition> Plans => m_plans;

        public abstract float EvaluateState(IBoardGameWorld world);
    }
}
