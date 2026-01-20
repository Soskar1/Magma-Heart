using MagmaHeart.AI.States;
using System;

namespace MagmaHeart.Core.Entities.Properties
{
    public record EnergyPropertySnapshot : PropertySnapshot
    {
        public int CurrentEnergy { get; init; }
        public int MaxEnergy { get; init; }

        public EnergyPropertySnapshot(int currentEnergy, int maxEnergy)
        {
            MaxEnergy = maxEnergy;
            CurrentEnergy = Math.Clamp(currentEnergy, 0, MaxEnergy);
        }
    }
}
