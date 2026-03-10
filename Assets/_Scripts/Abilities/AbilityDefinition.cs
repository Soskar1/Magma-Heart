using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Requirements;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Ability Definition")]
    public class AbilityDefinition : ScriptableObject
    {
        [SerializeField] private string m_id;
        [SerializeField] private TargetKind m_targetKind;
        [SerializeField] private int m_cooldownTurns;
        
        [SerializeReference, SubclassSelector]
        private List<CostModule> m_cost;

        [SerializeReference, SubclassSelector]
        private List<EffectModule> m_effects;

        [SerializeReference, SubclassSelector]
        private List<IAbilityRequirement> m_requirements;

        public string Id => m_id;
        public List<CostModule> Cost => m_cost;
        public List<EffectModule> Effects => m_effects;
        public List<IAbilityRequirement> Requirements => m_requirements;
        public TargetKind TargetKind => m_targetKind;
        public int CooldownTurns => m_cooldownTurns;
    }
}
