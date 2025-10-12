using System;
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
        [SerializeField] private List<StatModifierModelList> m_statModifierModels = new List<StatModifierModelList>()
        {
            new StatModifierModelList(),
            new StatModifierModelList(),
            new StatModifierModelList(),
            new StatModifierModelList(),
            new StatModifierModelList()
        };

        public Rarity Rarity { get => m_rarity; set => m_rarity = value; }
        public string Name { get => m_name; set => m_name = value; }
        public string Description { get => m_description; set => m_description = value; }
        public Sprite Sprite { get => m_sprite; set => m_sprite = value; }
        public List<StatModifierModelList> StatModifierModels { get => m_statModifierModels; }
        public int MaxLevel
        {
            get
            {
                return m_rarity switch
                {
                    Rarity.Common => 5,
                    Rarity.Rare => 5,
                    Rarity.Epic => 3,
                    Rarity.Legendary => 3,
                    Rarity.Magma => 2,
                    _ => 1,
                };
            }
        }

        public List<List<IStatModifier>> StatModifiers { get; private set; }
        public void Initialize()
        {
            StatModifiers = new List<List<IStatModifier>>();
            foreach (StatModifierModelList modelList in m_statModifierModels)
            {
                List<IStatModifier> modifiers = new List<IStatModifier>();
                foreach (StatModifierModel model in modelList.Models)
                {
                    Type type = Type.GetType(model.Type);
                    Debug.Log(model.Type);
                    IStatModifier modifier = (IStatModifier)Activator.CreateInstance(type, model.Value);
                    if (modifier != null)
                        modifiers.Add(modifier);
                }
                StatModifiers.Add(modifiers);
            }
        }
    }

    [Serializable]
    public class StatModifierModelList
    {
        [SerializeField] private List<StatModifierModel> m_models;

        public List<StatModifierModel> Models => m_models;

        public StatModifierModelList() => m_models = new List<StatModifierModel>();
    }
}