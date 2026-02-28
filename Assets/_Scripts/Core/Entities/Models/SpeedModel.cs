using MagmaHeart.Abilities;

namespace MagmaHeart.Core.Entities.Models
{
    public class SpeedModel : IParameter
    {
        private int m_currentSpeed;
        
        public ParameterId Id { get; init; }
        public float CurrentValue => CurrentSpeed;

        public int CurrentSpeed
        {
            get => m_currentSpeed;
            set => m_currentSpeed = value;
        }

        public SpeedModel(int initialSpeed, ParameterId id)
        {
            m_currentSpeed = initialSpeed;
            Id = id;
        }

        public SpeedModel DeepCopy() => new SpeedModel(m_currentSpeed, Id);

        public void SetValue(float value) => CurrentSpeed = (int)value;
    }
}
