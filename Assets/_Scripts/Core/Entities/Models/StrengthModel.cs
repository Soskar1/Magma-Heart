using System;
using MagmaHeart.Abilities;

namespace MagmaHeart.Core.Entities.Models
{
    public class StrengthModel : IParameter
    {
        private int m_currentStrength;

        public ParameterId Id { get; init; }
        public float CurrentValue => CurrentStrength;

        public int CurrentStrength
        {
            get => m_currentStrength;
            set
            {
                m_currentStrength = value;

                OnParameterValueChangedEventArgs args = new OnParameterValueChangedEventArgs(Id, CurrentStrength);
                OnParameterValueChanged?.Invoke(this, args);
            }
        }

        public event EventHandler<OnParameterValueChangedEventArgs> OnParameterValueChanged;

        public StrengthModel(int initialStrength, ParameterId id)
        {
            m_currentStrength = initialStrength;
            Id = id;
        }

        public StrengthModel DeepCopy() => new StrengthModel(m_currentStrength, Id);

        public void SetValue(float value) => CurrentStrength = (int)value;
    }
}
