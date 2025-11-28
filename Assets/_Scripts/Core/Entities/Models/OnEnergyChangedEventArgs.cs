using System;

namespace MagmaHeart.Core.Entities.Models
{
    public class OnEnergyChangedEventArgs : EventArgs
    {
        public int CurrentEnergy { get; init; }
        public int MaxEnergy { get; init; }

        public OnEnergyChangedEventArgs(int currentEnergy, int maxEnergy)
        {
            CurrentEnergy = currentEnergy;
            MaxEnergy = maxEnergy;
        }
    }
}
