using MagmaHeart.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    [CreateAssetMenu(menuName = "Magma Heart Data/Artifacts/Artifact Data")]
    public class ArtifactData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private string m_description;
        [SerializeField] private Rarity m_rarity;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private Sprite m_abilityIcon;
        [SerializeField] private List<ArtifactLevelDefinition> m_levelDefinitions;
        [SerializeField] private AbilityDefinition m_abilityDefinition;
        [SerializeField] private ParticleSystem m_abilityWindowVfx;

        public string Name => m_name;
        public string Description => m_description;
        public ParticleSystem AbilityWindowVfx => m_abilityWindowVfx;
        public Rarity Rarity => m_rarity;
        public Sprite Icon => m_icon;
        public Sprite AbilityIcon => m_abilityIcon;
        public IReadOnlyList<ArtifactLevelDefinition> LevelDefinitions => m_levelDefinitions;
        public int MaxLevel => m_levelDefinitions.Count;
        public AbilityDefinition AbilityDefinition => m_abilityDefinition;
    }
}