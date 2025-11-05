using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using System;

namespace MagmaHeart.Core.Entities.CombatSystem
{
    public class CombatController
    {
        public event EventHandler NextTurn;
        public event EventHandler<OnCombatStartedEventArgs> OnCombatStarted;
        public event EventHandler OnTurnStarted;
        public event EventHandler OnTurnEnded;

        public void StartCombat(Room room)
        {
            OnCombatStartedEventArgs args = new OnCombatStartedEventArgs(room);
            OnCombatStarted?.Invoke(this, args);
        }

        public void StartTurn() => OnTurnStarted?.Invoke(this, EventArgs.Empty);
        public void EndTurn()
        {
            OnTurnEnded?.Invoke(this, EventArgs.Empty);
            NextTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}