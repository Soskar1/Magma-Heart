using MagmaHeart.Core.Dungeon;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnBattleStartedEventArgs : EventArgs
    {
        public Room Room { get; init; }
        public OnBattleStartedEventArgs(Room room) => Room = room;
    }
}
