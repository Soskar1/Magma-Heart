using System;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Services;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleInstaller : IInstaller
    {
        private BattleInitializer m_battleInitializer;

        public BattleContext Install(MagmaHeartServices services, AIContext aiContext, Random random, RoomGrid grid, float minDistanceFromPlayer, GameWorld gameWorld, ActionExecutor actionRunner)
        {
            Battle battle = new Battle(aiContext.StartOfTurnCommandFactory, actionRunner);
            m_battleInitializer = new BattleInitializer(services.SpawnService.EntitySpawner, aiContext.AiEngine, random, grid, minDistanceFromPlayer, gameWorld, actionRunner);

            return new BattleContext(battle, m_battleInitializer);
        }

        public void Dispose()
        {
            m_battleInitializer.Dispose();
        }
    }
}