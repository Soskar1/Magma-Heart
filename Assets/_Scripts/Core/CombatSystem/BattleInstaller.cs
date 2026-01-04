using System;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleInstaller : IInstaller
    {
        public BattleContext Install(MagmaHeartSpawner spawner, EntityMovementService movementService, AIEngine aiEngine, Random random, float minDistanceFromPlayer)
        {
            Battle battle = new Battle(spawner, movementService);
            BattleInitializer initializer = new BattleInitializer(spawner.EntitySpawner, aiEngine, random, minDistanceFromPlayer);

            ArtifactDatabase database = new ArtifactDatabase();
            BattleReward battleReward = new BattleReward(database);

            return new BattleContext(battle, initializer, battleReward);
        }

        public void Dispose() { }
    }
}