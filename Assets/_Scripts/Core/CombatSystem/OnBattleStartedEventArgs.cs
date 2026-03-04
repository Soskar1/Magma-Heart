using MagmaHeart.Core.Dungeon;
using System;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnBattleStartedEventArgs : EventArgs
    {
        public TurnOrder TurnOrder { get; init; }
        public Room Room { get; init; }

        public OnBattleStartedEventArgs(TurnOrder turnOrder, Room room)
        {
            TurnOrder = turnOrder;
            Room = room;
        }
    }
}
