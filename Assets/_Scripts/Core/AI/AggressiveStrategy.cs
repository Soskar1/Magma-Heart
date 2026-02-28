using MagmaHeart.AI;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    public class AggressiveStrategy : Strategy
    {
        private const float PLAYER_HEALTH_WEIGHT = 0.8f;
        private const float DISTANCE_WEIGHT = 0.1f;
        private const float AI_IS_NOT_ALIVE_POINTS = -50;

        public AggressiveStrategy() {
            //Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
            //    new PlanTaskDefinition(typeof(MovementAction))
            //}));

            //Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
            //    new PlanTaskDefinition(typeof(AttackAction), true),
            //}));

            //Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
            //    new PlanTaskDefinition(typeof(MovementAction)),
            //    new PlanTaskDefinition(typeof(AttackAction), true)
            //}));
        }

        public override float EvaluateState(IBoardGameWorld world)
        {
            // !IS_ALIVE == -50 if AI
            // !IS_ALIVE == 100 if PLAYER
            // 1 * (PLAYER_IS_NOT_ALIVE + AI_IS_NOT_ALIVE) + w1 * PLAYER_HP_DIFF + w2 * (5 / AI_DISTANCE_TO_PLAYER)

            float aiHP = 0;
            float distancePoints = 0;
            float playerIsNotAlivePoints = 0;
            int aiNotAliveCount = 0;
            EntityModel player = null;
            IList<AIUnitModel> allUnits = world.GetUnits().ToList();

            foreach (AIUnitModel unitModel in allUnits)
            {
                if (unitModel.IsPlayer)
                {
                    player = (EntityModel)unitModel;
                    break;
                }
            }

            float playerHealthDifference = player.Health.MaxHealth - player.Health.CurrentHealth;

            if (player.IsDisabled)
                playerIsNotAlivePoints = 100;

            Func<Vector3Int, float> getDistancePoints = (ai) =>
            {
                float distance = Vector3Int.Distance(ai, player.TilePosition);
                if (distance == 0)
                    return 5;

                return 5 / distance;
            };

            foreach (AIUnitModel unit in allUnits)
            {
                if (unit.IsPlayer)
                    continue;

                if (unit.IsDisabled)
                {
                    ++aiNotAliveCount;
                }
                else
                {
                    aiHP += aiEntity.Health.CurrentHealth;

                    if (!player.IsDisabled)
                        distancePoints += getDistancePoints(aiEntity.TilePosition);
                }
            }

            return playerIsNotAlivePoints
                + AI_IS_NOT_ALIVE_POINTS * aiNotAliveCount
                + PLAYER_HEALTH_WEIGHT * playerHealthDifference
                + DISTANCE_WEIGHT * distancePoints;
        }
    }
}
