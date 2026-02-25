using MagmaHeart.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution
{
    [CreateAssetMenu(menuName = "Magma Heart Data/Ability Execution/Script")]
    public class AbilityExecutionScript : ScriptableObject
    {
        [SerializeField] private AbilityDefinition m_ability;

        [SerializeReference, SubclassSelector]
        private List<IAbilityExecutionStep> m_steps;

        public AbilityDefinition Ability => m_ability;
        public IReadOnlyList<IAbilityExecutionStep> Steps => m_steps;
    }
}
