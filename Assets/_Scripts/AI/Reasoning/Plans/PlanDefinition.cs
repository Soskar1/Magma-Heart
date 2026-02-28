using MagmaHeart.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Plans
{
    [CreateAssetMenu(menuName = "AI/Plan Definition")]
    public class PlanDefinition : ScriptableObject
    {
        [SerializeField] private List<PlanTask> m_taskDefinitions;

        public List<PlanTask> TaskDefinitions => m_taskDefinitions;
    }

    [System.Serializable]
    public class PlanTask
    {
        [SerializeField] private AbilityDefinition m_ability;
        [SerializeField] private bool m_executeUntilFail;
        
        [SerializeReference, SubclassSelector]
        private ITargetSelector m_targetSelector;

        public AbilityDefinition Ability => m_ability;
        public bool ExecuteUntilFail => m_executeUntilFail;
        public ITargetSelector TargetSelector => m_targetSelector;
    }
}
