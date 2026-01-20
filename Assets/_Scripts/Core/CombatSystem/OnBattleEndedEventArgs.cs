using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnBattleEndedEventArgs : EventArgs
    {
        public bool IsPlayerVictory { get; init; }

        public OnBattleEndedEventArgs(bool isPlyaerVictory) => IsPlayerVictory = isPlyaerVictory;
    }
}
