using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    [CreateAssetMenu(fileName = "ArtifactData", menuName = "MagmaHeartData/ArtifactData")]
    public class ArtifactData : ScriptableObject
    {
        [SerializeField] private Rarity m_rarity;
        [SerializeField] private string m_name;
        [SerializeField] private string m_description; 
        [SerializeField] private Sprite m_sprite;
        [SerializeField] private List<List<IStatModifier>> m_statModifiers = new List<List<IStatModifier>>()
        {
            new List<IStatModifier>(),
            new List<IStatModifier>(),
            new List<IStatModifier>(),
            new List<IStatModifier>(),
            new List<IStatModifier>()
        };

        public Rarity Rarity { get => m_rarity; set => m_rarity = value; }
        public string Name { get => m_name; set => m_name = value; }
        public string Description { get => m_description; set => m_description = value; }
        public Sprite Sprite { get => m_sprite; set => m_sprite = value; }
        public List<List<IStatModifier>> StatModifiers { get => m_statModifiers; }
    }
}