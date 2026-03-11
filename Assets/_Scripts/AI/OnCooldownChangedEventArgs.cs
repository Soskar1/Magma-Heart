using System;

namespace MagmaHeart.AI
{
    public class OnCooldownChangedEventArgs : EventArgs
    {
        public string AbilityId { get; }
        public int CurrentCooldown { get; }
        public OnCooldownChangedEventArgs(string abilityId, int cooldown)
        {
            AbilityId = abilityId;
            CurrentCooldown = cooldown;
        }
    }
}