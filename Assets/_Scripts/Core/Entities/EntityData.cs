using MagmaHeart.Abilities;
using MagmaHeart.AI.Actions;
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
        [SerializeField] private List<ActionData> m_actions;
        [SerializeField] private RuntimeAnimatorController m_animatorController;
        [SerializeField] private List<AbilityDefinition> m_additionalAbilities;
        [SerializeField] private AbilityDefinition m_attackAbility;
        [SerializeField] private AbilityDefinition m_movementAbility;

        public string Name => m_name;
        public Sprite PortraitImage => m_portraitImage;
        public EntityStats Stats => m_stats;
        public List<ActionData> Actions => m_actions;
        public List<AbilityDefinition> AdditionalAbilities => m_additionalAbilities;
        public AbilityDefinition AttackAbility => m_attackAbility;
        public AbilityDefinition MovementAbility => m_movementAbility;
        public RuntimeAnimatorController AnimatorController => m_animatorController;

        public EntityData(string name, EntityStats stats, List<ActionData> actions)
        {
            m_name = name;
            m_stats = stats;
            m_actions = actions;
        }
    }
}