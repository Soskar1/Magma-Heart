using System;

namespace MagmaHeart.Abilities
{
    public interface IParameter
    {
        public ParameterId Id { get; }
        public float CurrentValue { get; }

        public event EventHandler<OnParameterValueChangedEventArgs> OnParameterValueChanged;

        public void SetValue(float value);
    }
}
