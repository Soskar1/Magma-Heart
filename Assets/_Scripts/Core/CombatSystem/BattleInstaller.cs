using System;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Services;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleInstaller : IInstaller
    {
        public BattleContext Install(MagmaHeartServices services, AIContext aiContext, Random random, RoomGrid grid, float minDistanceFromPlayer)
        {
            Battle battle = new Battle(services, aiContext.TurnContext);
            BattleInitializer initializer = new BattleInitializer(services.SpawnService.EntitySpawner, aiContext.AiEngine, random, grid, minDistanceFromPlayer);

            ArtifactDatabase database = new ArtifactDatabase();
            BattleReward battleReward = new BattleReward(database);

            return new BattleContext(battle, initializer, battleReward);
        }

        public void Dispose() { }
    }
}