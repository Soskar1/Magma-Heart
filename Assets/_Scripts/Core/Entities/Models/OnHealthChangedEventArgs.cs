using System;

namespace MagmaHeart.Core.Entities.Models
{
    public class OnHealthChangedEventArgs : EventArgs
    {
        public float CurrentHealth { get; init; }
        public float MaxHealth { get; init; }

        public OnHealthChangedEventArgs(float currentHealth, float maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
}