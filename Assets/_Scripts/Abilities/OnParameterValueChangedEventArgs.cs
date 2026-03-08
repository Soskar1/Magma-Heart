using System;

namespace MagmaHeart.Abilities
{
    public class OnParameterValueChangedEventArgs : EventArgs
    {
        public ParameterId ParameterId { get; init; }
        public float NewValue { get; init; }

        public OnParameterValueChangedEventArgs(ParameterId parameterId, float newValue)
        {
            ParameterId = parameterId;
            NewValue = newValue;
        }
    }
}
