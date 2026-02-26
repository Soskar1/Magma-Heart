using System;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleInstaller : IInstaller
    {
        private BattleInitializer m_battleInitializer;

        public BattleContext Install(
            EntitySpawner spawner,
            AIContext aiContext,
            Random random,
            float minDistanceFromPlayer,
            GameWorld gameWorld,
            PlayerTurnController playerTurnController)
        {
            EnemyTurnController enemyTurnController = new EnemyTurnController(aiContext.AiEngine);

            Battle battle = new Battle(aiContext.StartOfTurnCommandFactory, playerTurnController, enemyTurnController);
            m_battleInitializer = new BattleInitializer(spawner, random, minDistanceFromPlayer, gameWorld);

            return new BattleContext(battle, m_battleInitializer);
        }

        public void Dispose()
        {
            m_battleInitializer.Dispose();
        }
    }
}