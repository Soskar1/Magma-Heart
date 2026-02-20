using MagmaHeart.Abilities.Effects;
using MagmaHeart.Abilities.Requirements;
using MagmaHeart.Abilities.Resources;
using MagmaHeart.Abilities.Targeting;
using UnityEngine;

namespace MagmaHeart.Abilities
{
    [CreateAssetMenu(menuName = "Actions/Action Definition")]
    public class AbilityDefinition : ScriptableObject
    {
        [SerializeField] private string m_id;
        [SerializeField] private TargetingModule m_targeting;
        [SerializeField] private CostModule m_cost;
        [SerializeField] private RequirementModule[] m_requirements;
        [SerializeField] private EffectModule[] m_effects;

        public string Id => m_id;
        public TargetingModule Targeting => m_targeting;
        public CostModule Cost => m_cost;
        public RequirementModule[] Requirements => m_requirements;
        public EffectModule[] Effects => m_effects;
    }
}
