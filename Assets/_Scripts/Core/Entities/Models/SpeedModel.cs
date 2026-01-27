namespace MagmaHeart.Core.Entities.Models
{
    public class SpeedModel
    {
        private int m_currentSpeed;

        public int CurrentSpeed
        {
            get => m_currentSpeed;
            set => m_currentSpeed = value;
        }

        public SpeedModel(int initialSpeed) => m_currentSpeed = initialSpeed;

        public SpeedModel DeepCopy() => new SpeedModel(m_currentSpeed);
    }
}
