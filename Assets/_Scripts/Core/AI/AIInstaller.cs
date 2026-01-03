using MagmaHeart.AI.Actions;
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
            MagmaHeartTurnContext turnContext = new MagmaHeartTurnContext();
            AIEngine aiEngine = new AIEngine(strategy, database, 2, turnContext);
            return new AIContext(database, aiEngine, turnContext);
        }

        public void Dispose() { }
    }
}
