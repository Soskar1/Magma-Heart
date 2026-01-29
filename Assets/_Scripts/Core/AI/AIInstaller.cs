using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Execution;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;
using System.Reflection;

namespace MagmaHeart.Core.AI
{
    public class AIInstaller : IInstaller
    {
        public AIContext Install()
        {
            ActionDatabase database = new ActionDatabase(Assembly.GetExecutingAssembly());
            AggressiveStrategy strategy = new AggressiveStrategy();
            IStartOfTurnCommandFactory factory = new StartOfTurnCommandFactory();

            AIEngine aiEngine = new AIEngine(strategy, database, 2, factory);
            return new AIContext(database, aiEngine, factory);
        }

        public void Dispose() { }
    }
}
