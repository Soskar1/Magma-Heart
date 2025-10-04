using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnTurnSwitchedEventArgs : EventArgs
    {
        public ICombatController Entity { get; init; }

        public OnTurnSwitchedEventArgs(ICombatController entity) => Entity = entity;
    }
}