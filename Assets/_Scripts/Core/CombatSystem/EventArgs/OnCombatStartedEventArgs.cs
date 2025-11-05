using MagmaHeart.Core.Dungeon;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnCombatStartedEventArgs : EventArgs
    {
        public Room Room { get; init; }
        public OnCombatStartedEventArgs(Room room) => Room = room;
    }
}
