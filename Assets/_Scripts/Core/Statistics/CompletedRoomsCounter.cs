using System;
using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.Statistics
{
    public class CompletedRoomsCounter : IDisposable
    {
        private readonly DungeonController m_dungeon;
        private int m_completedRooms = 0;

        public event EventHandler<OnCompletedRoomsCounterChangedEventArgs> OnCompletedRoomsCounterChanged;

        public int CompletedRooms
        {
            get => m_completedRooms;
            set
            {
                m_completedRooms = value;

                OnCompletedRoomsCounterChangedEventArgs args = new OnCompletedRoomsCounterChangedEventArgs(m_completedRooms);
                OnCompletedRoomsCounterChanged?.Invoke(this, args);
            }
        }

        public CompletedRoomsCounter(DungeonController dungeon)
        {
            m_dungeon = dungeon;
            m_dungeon.OnRoomChanged += HandleOnRoomChanged;
        }

        public void Dispose() => m_dungeon.OnRoomChanged -= HandleOnRoomChanged;

        private void HandleOnRoomChanged(object _, EventArgs __) => ++CompletedRooms;
    }
}