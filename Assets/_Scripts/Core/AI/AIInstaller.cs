using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.AI
{
    public class AIInstaller : IInstaller
    {
        public AIContext Install(Strategy strategy, IStartOfTurnEffectFactory factory, EffectDispatcher effectDispatcher, int lookAhead)
        {
            AIEngine aiEngine = new AIEngine(strategy, lookAhead, factory, effectDispatcher);
            return new AIContext(aiEngine);
        }

        public void Dispose() { }
    }
}
