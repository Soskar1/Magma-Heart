namespace MagmaHeart.Core.Entities.Models
{
    public class StrengthModel
    {
        private int m_currentStrength;

        public int CurrentStrength
        {
            get => m_currentStrength;
            set => m_currentStrength = value;
        }

        public StrengthModel(int initialStrength) => m_currentStrength = initialStrength;
    }
}
