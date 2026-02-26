using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.AI
{
    public class AIInstaller : IInstaller
    {
        public AIContext Install()
        {
            AggressiveStrategy strategy = new AggressiveStrategy();
            // IStartOfTurnCommandFactory factory = new StartOfTurnCommandFactory();

            AIEngine aiEngine = new AIEngine(strategy, 2);
            return new AIContext(aiEngine);
        }

        public void Dispose() { }
    }
}
