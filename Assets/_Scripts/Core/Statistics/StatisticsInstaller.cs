using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.Statistics
{
    public class StatisticsInstaller : IInstaller
    {
        private CompletedRoomsCounter m_completedRoomsCounter;

        public CompletedRoomsCounter Install(GameWorld gameWorld)
        {
            m_completedRoomsCounter = new CompletedRoomsCounter(gameWorld);
            return m_completedRoomsCounter;
        }

        public void Dispose()
        {
            m_completedRoomsCounter.Dispose();
        }
    }
}