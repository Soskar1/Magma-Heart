using MagmaHeart.Abilities;
using System;

namespace MagmaHeart.Core.Entities.Models
{
    public class MagmaHeartModel : IParameter
    {
        private int m_currentMagmaHeartCount;

        public ParameterId Id { get; init; }

        public float CurrentValue => CurrentMagmaHeartCount;
        public int CurrentMagmaHeartCount
        {
            get => m_currentMagmaHeartCount;
            set
            {
                m_currentMagmaHeartCount = value;
                OnParameterValueChangedEventArgs args = new OnParameterValueChangedEventArgs(Id, CurrentValue);
                OnParameterValueChanged?.Invoke(this, args);
            }
        }

        public event EventHandler<OnParameterValueChangedEventArgs> OnParameterValueChanged;

        public MagmaHeartModel(ParameterId id)
        {
            m_currentMagmaHeartCount = 0;
            Id = id;
        }

        public void SetValue(float value)
        {
            CurrentMagmaHeartCount = (int)value;
        }
    }
}
