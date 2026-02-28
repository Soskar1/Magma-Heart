using MagmaHeart.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution
{
    [CreateAssetMenu(menuName = "Magma Heart Data/Ability Execution/Script Database")]
    public class AbilityExecutionScriptDatabase : ScriptableObject
    {
        [SerializeField] private List<AbilityExecutionScript> m_scripts;

        private Dictionary<AbilityDefinition, AbilityExecutionScript> m_lookup;

        private void OnEnable()
        {
            m_lookup = new Dictionary<AbilityDefinition, AbilityExecutionScript>(m_scripts.Count);
            foreach (AbilityExecutionScript script in m_scripts)
            {
                if (script != null && script.Ability != null)
                    m_lookup[script.Ability] = script;
            }
        }

        public bool TryGetValidScript(AbilityDefinition ability, AbilityPlan plan, out AbilityExecutionScript script)
        {
            script = null;

            if (!m_lookup.TryGetValue(ability, out AbilityExecutionScript candidate))
            {
                Debug.LogWarning($"No execution script found for ability {plan.AbilityDefinition.Id}. Applying effects directly.");
                return false;
            }

            if (!candidate.IsValid(plan))
                return false;

            script = candidate;
            return true;
        }
    }
}