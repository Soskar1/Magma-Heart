using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Energy
    {
        private int m_maxEnergy;
        private int m_energyRegenerationPerTurn;

        public int CurrentEnergy { get; private set; }

        public Energy(int maxEnergy, int energyRegenerationPerTurn)
        {
            m_maxEnergy = maxEnergy;
            CurrentEnergy = 0;
            m_energyRegenerationPerTurn = energyRegenerationPerTurn;
        }

        public bool HasEnough(int energyToSpent) => energyToSpent >= CurrentEnergy;

        public void Spent(int amount)
        {
            if (!HasEnough(amount))
            {
                Debug.LogWarning($"Tried to spent {amount}, but entity have {CurrentEnergy}");
                return;
            }

            CurrentEnergy -= amount;
        }

        public void Regenerate()
        {
            CurrentEnergy += m_energyRegenerationPerTurn;

            if (CurrentEnergy > m_maxEnergy)
                CurrentEnergy = m_maxEnergy;
        }
    }
}