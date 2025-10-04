using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleEndedEventArgs : EventArgs
    {
        public List<ICombatController> BattleSurvivors { get; }
        public bool IsPlayerVictory { get; }
        public BattleEndedEventArgs(bool isPlayerVictory, List<ICombatController> entities)
        {
            IsPlayerVictory = isPlayerVictory;
            BattleSurvivors = entities;
        }
    }
}