using System;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    [Serializable]
    public class StatModifierModel
    {
        [SerializeField] private string m_modifierName;
        [SerializeField] private float m_value;
        [SerializeField] private string m_type;

        public string ModifierName => m_modifierName;
        public float Value
        {
            get => m_value;
            set => m_value = value;
        }
        public string Type
        {
            get => m_type;
            private set => m_type = value;
        }

        public StatModifierModel(string modifierName, Type type, float value)
        {
            m_modifierName = modifierName;
            m_value = value;
            Type = type.FullName;
        }
    }
}
