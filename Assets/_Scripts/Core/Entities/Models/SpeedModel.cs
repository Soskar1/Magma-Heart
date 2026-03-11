using System;
using MagmaHeart.Abilities;

namespace MagmaHeart.Core.Entities.Models
{
    public class SpeedModel : IParameter
    {
        public ParameterId Id { get; init; }
        private int m_currentSpeed;

        public float CurrentValue => CurrentSpeed;

        public int CurrentSpeed
        {
            get => m_currentSpeed;
            set
            {
                m_currentSpeed = value;

                OnParameterValueChangedEventArgs args = new OnParameterValueChangedEventArgs(Id, CurrentValue);
                OnParameterValueChanged?.Invoke(this, args);
            }
        }

        public event EventHandler<OnParameterValueChangedEventArgs> OnParameterValueChanged;

        public SpeedModel(int initialSpeed, ParameterId id)
        {
            m_currentSpeed = initialSpeed;
            Id = id;
        }

        public SpeedModel DeepCopy() => new SpeedModel(m_currentSpeed, Id);

        public void SetValue(float value) => CurrentSpeed = (int)value;
    }
}
