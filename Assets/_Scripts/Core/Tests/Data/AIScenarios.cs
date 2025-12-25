using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal static class AIScenarios
    {
        private static (EnemyCombatController, EnemyCombatController) CreatePlayerAndEnemy(CombatBoardState state, Vector3Int playerPosition, Vector3Int enemyPosition, List<ActionData> enemyActions)
        {
            EntityInitializationData playerData = new EntityInitializationData(playerPosition, true, 5, ActionPresets.MeleeAttacker);
            EntityInitializationData enemyData = new EntityInitializationData(enemyPosition, false, 5, enemyActions);

            EntityModel player = BoardBuilder.AddEntity(state.Board, playerData);
            EntityModel enemy = BoardBuilder.AddEntity(state.Board, enemyData);

            EnemyCombatController playerContext = new EnemyCombatController(player, null);
            EnemyCombatController enemyContext = new EnemyCombatController(enemy, null);

            return (playerContext, enemyContext);
        }

        public static AIScenario PlayerAdjacentToEnemy(CombatBoardState state, List<ActionData> enemyActions)
        {
            (var playerContext, var enemyContext) = CreatePlayerAndEnemy(state, new Vector3Int(2, 3), new Vector3Int(3, 3), enemyActions);

            return new AIScenario(state, new TurnOrder(new[] { enemyContext, playerContext }), playerContext.TypedModel);
        }

        public static AIScenario PlayerIsFarAwayFromEnemy(CombatBoardState state, List<ActionData> enemyActions)
        {
            (var playerContext, var enemyContext) = CreatePlayerAndEnemy(state, new Vector3Int(9, 9), new Vector3Int(0, 0), enemyActions);

            return new AIScenario(state, new TurnOrder(new[] { enemyContext, playerContext }), playerContext.TypedModel);
        }

        public static AIScenario EnemySurroundedByWalls(CombatBoardState state, List<ActionData> enemyActions)
        {
            (var playerContext, var enemyContext) = CreatePlayerAndEnemy(state, new Vector3Int(2, 3), new Vector3Int(4, 3), enemyActions);

            BoardBuilder.SurroundWithWalls(state.Board, enemyContext.TypedModel);

            return new AIScenario(state, new TurnOrder(new[] { enemyContext, playerContext }), playerContext.TypedModel);
        }
    }
}