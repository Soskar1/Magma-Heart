using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.Statistics
{
    public class StatisticsInstaller : IInstaller
    {
        private CompletedRoomsCounter m_completedRoomsCounter;

        public CompletedRoomsCounter Install(DungeonController dungeon)
        {
            m_completedRoomsCounter = new CompletedRoomsCounter(dungeon);
            return m_completedRoomsCounter;
        }

        public void Dispose()
        {
            m_completedRoomsCounter.Dispose();
        }
    }
}