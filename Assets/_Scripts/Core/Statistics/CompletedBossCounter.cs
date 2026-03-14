using System;

namespace MagmaHeart.Core.Statistics
{
    public class CompletedBossCounter
    {
        private readonly GameWorld m_gameWorld;
        private int m_completedBosses = 0;

        public event EventHandler<int> OnCompleteBossCounterChanged;

        public int CompletedRooms
        {
            get => m_completedBosses;
            set
            {
                m_completedBosses = value;
                OnCompleteBossCounterChanged?.Invoke(this, m_completedBosses);
            }
        }

        public CompletedBossCounter(GameWorld gameWorld)
        {
            m_gameWorld = gameWorld;
            m_gameWorld.OnLocationChanged += HandleOnLocationChanged;
        }

        public void Dispose() => m_gameWorld.OnLocationChanged -= HandleOnLocationChanged;

        private void HandleOnLocationChanged(object _, EventArgs __) => ++CompletedRooms;
    }
}
