using System;

namespace MagmaHeart.Core.Statistics
{
    public class OnCompletedRoomsCounterChangedEventArgs : EventArgs
    {
        public int CompletedRooms { get; init; }
        public OnCompletedRoomsCounterChangedEventArgs(int completedRooms) => CompletedRooms = completedRooms;
    }
}