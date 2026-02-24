using MagmaHeart.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution
{
    public abstract class AbilityExecutionScript : ScriptableObject
    {
        [SerializeField] private AbilityDefinition m_ability;
        public AbilityDefinition Ability => m_ability;

        public abstract List<IAbilityExecutionStep> BuildSteps(AbilityPlan plan, int executorId);
    }
}
