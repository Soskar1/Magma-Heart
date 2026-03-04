using MagmaHeart.Abilities;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.Core.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Magma Heart Data/Entity Data")]
    public class EntityData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_portraitImage;
        [SerializeField] private EntityStats m_stats;
        [SerializeField] private RuntimeAnimatorController m_animatorController;
        [SerializeField] private AbilityDefinition m_attackAbility;
        [SerializeField] private AbilityDefinition m_movementAbility;

        [SerializeField] private ParameterDatabase m_parameterDatabase;

        [Header("AI")]
        [SerializeField] private List<PlanDefinition> m_plans;

        public string Name => m_name;
        public Sprite PortraitImage => m_portraitImage;
        public EntityStats Stats => m_stats;
        public AbilityDefinition AttackAbility => m_attackAbility;
        public AbilityDefinition MovementAbility => m_movementAbility;
        public RuntimeAnimatorController AnimatorController => m_animatorController;
        public ParameterDatabase ParameterDatabase => m_parameterDatabase;
        public IReadOnlyList<PlanDefinition> Plans => m_plans;
    }
}