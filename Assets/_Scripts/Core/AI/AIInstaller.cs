using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.AI
{
    public class AIInstaller : IInstaller
    {
        public AIContext Install(IStartOfTurnEffectFactory factory, EffectDispatcher effectDispatcher)
        {
            AggressiveStrategy strategy = new AggressiveStrategy();

            AIEngine aiEngine = new AIEngine(strategy, 2, factory, effectDispatcher);
            return new AIContext(aiEngine);
        }

        public void Dispose() { }
    }
}
