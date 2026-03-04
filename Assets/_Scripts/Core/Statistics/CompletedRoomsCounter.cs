using System;

namespace MagmaHeart.Core.Statistics
{
    public class CompletedRoomsCounter : IDisposable
    {
        private readonly GameWorld m_gameWorld;
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

        public CompletedRoomsCounter(GameWorld gameWorld)
        {
            m_gameWorld = gameWorld;
            m_gameWorld.OnRoomChanged += HandleOnRoomChanged;
        }

        public void Dispose() => m_gameWorld.OnRoomChanged -= HandleOnRoomChanged;

        private void HandleOnRoomChanged(object _, EventArgs __) => ++CompletedRooms;
    }
}