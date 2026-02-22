using MagmaHeart.Abilities;

namespace MagmaHeart.Core.Entities.Models
{
    public class StrengthModel : IParameter
    {
        private int m_currentStrength;
        
        public ParameterId Id { get; init; }
        public float CurrentValue
        {
            get => CurrentStrength;
            set => CurrentStrength = (int)value;
        }

        public int CurrentStrength
        {
            get => m_currentStrength;
            set => m_currentStrength = value;
        }

        public StrengthModel(int initialStrength, ParameterId id)
        {
            m_currentStrength = initialStrength;
            Id = id;
        }

        public StrengthModel DeepCopy() => new StrengthModel(m_currentStrength, Id);
    }
}
