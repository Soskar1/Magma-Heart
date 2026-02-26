using MagmaHeart.AI.Execution;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.AI
{
    public class AIInstaller : IInstaller
    {
        public AIContext Install()
        {
            AggressiveStrategy strategy = new AggressiveStrategy();
            IStartOfTurnCommandFactory factory = new StartOfTurnCommandFactory();

            AIEngine aiEngine = new AIEngine(strategy, 2, factory);
            return new AIContext(aiEngine, factory);
        }

        public void Dispose() { }
    }
}
