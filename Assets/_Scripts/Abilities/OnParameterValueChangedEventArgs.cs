using System;

namespace MagmaHeart.Abilities
{
    public class OnParameterValueChangedEventArgs : EventArgs
    {
        public ParameterId ParameterId { get; init; }
        public float PreviousValue { get; init; }
        public float NewValue { get; init; }

        public OnParameterValueChangedEventArgs(ParameterId parameterId, float newValue, float previousValue)
        {
            ParameterId = parameterId;
            NewValue = newValue;
            PreviousValue = previousValue;
        }
    }
}
