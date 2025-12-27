using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
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
            CombatAI combatAI = new CombatAI(strategy, database, 2);
            return new AIContext(database, combatAI);
        }

        public void Dispose() { }
    }
}
