using MagmaHeart.DungeonGeneration;
using System;

namespace MagmaHeart.Core.Dungeon
{
    public class OnRoomGeneratedEventArgs : EventArgs
    {
        public Room Room { get; init; }

        public OnRoomGeneratedEventArgs(Room room) => Room = room;
    }
}
