using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.Statistics
{
    public class StatisticsInstaller : IInstaller
    {
        private CompletedRoomsCounter m_completedRoomsCounter;
        private CompletedBossCounter m_completedBossCounter;

        public (CompletedRoomsCounter roomCounter, CompletedBossCounter bossCounter) Install(GameWorld gameWorld)
        {
            m_completedRoomsCounter = new CompletedRoomsCounter(gameWorld);
            m_completedBossCounter = new CompletedBossCounter(gameWorld);
            return (m_completedRoomsCounter, m_completedBossCounter);
        }

        public void Dispose()
        {
            m_completedRoomsCounter.Dispose();
            m_completedBossCounter.Dispose();
        }
    }
}