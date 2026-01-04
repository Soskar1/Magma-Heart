using System;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Services;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleInstaller : IInstaller
    {
        public BattleContext Install(MagmaHeartServices services, AIEngine aiEngine, Random random, RoomGrid grid, float minDistanceFromPlayer)
        {
            Battle battle = new Battle(services);
            BattleInitializer initializer = new BattleInitializer(services.SpawnService.EntitySpawner, aiEngine, random, grid, minDistanceFromPlayer);

            ArtifactDatabase database = new ArtifactDatabase();
            BattleReward battleReward = new BattleReward(database);

            return new BattleContext(battle, initializer, battleReward);
        }

        public void Dispose() { }
    }
}