using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class BasicStrategy : Strategy
    {
        private const float HEALTH_WEIGHT = 0.8f;
        private const float DISTANCE_WEIGHT = 0.1f;

        public BasicStrategy() {
            Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
                new PlanTaskDefinition(typeof(MoveAction))
            }));

            Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
                new PlanTaskDefinition(typeof(AttackAction))
            }));

            Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
                new PlanTaskDefinition(typeof(RunAwayAction))
            }));
        }

        public override float EvaluateState(SimulatedBoardState state)
        {
            // !IS_ALIVE == -50 if AI
            // !IS_ALIVE == 100 if PLAYER
            // 1 * (PLAYER_IS_NOT_ALIVE + AI_IS_NOT_ALIVE) + w1 * (AI_HP - PLAYER_HP) + w2 * (5 / AI_DISTANCE_TO_PLAYER)

            float aiHP = 0;
            float playerHP = 0;
            float distancePoints = 0;
            bool playerIsAlive = true;
            float playerIsNotAlivePoints = 0;
            float aiIsNotAlivePoints = -50;
            int aiNotAliveCount = 0;
            Vector2 playerPosition = Vector2.zero;
            
            Func<Vector2, float> getDistancePoints = (pos) => {
                float distance = Vector2.Distance(pos, playerPosition);
                if (distance == 0)
                    return 5;

                return 5 / distance;
            };
            
            IEnumerable<AIUnitModel> aiUnits = state.Board.GetUnits();
            foreach (AIUnitModel unit in aiUnits)
            {
                Entity entity = (Entity)unit;

                if (unit.IsPlayer)
                {
                    if (!entity.IsDisabled)
                    {
                        playerIsAlive = true;
                        playerHP = entity.CurrentHealth;
                        playerPosition = entity.Position;
                    }
                    else
                    {
                        playerIsNotAlivePoints = 100;
                    }
                }
                else
                {
                    if (entity.CurrentHealth <= 0)
                    {
                        ++aiNotAliveCount;
                    }
                    else
                    {
                        aiHP += entity.CurrentHealth;
                    }
                }
            }

            if (playerIsAlive)
            {
                foreach (AIUnitModel unit in aiUnits)
                {
                    if (unit.IsPlayer)
                        continue;

                    Entity entity = (Entity)unit;
                    if (entity.IsDisabled)
                        continue;

                    distancePoints += getDistancePoints(entity.Position);
                }
            }

            return playerIsNotAlivePoints
                + aiIsNotAlivePoints * aiNotAliveCount
                + HEALTH_WEIGHT * (aiHP - playerHP)
                + DISTANCE_WEIGHT * distancePoints;
        }
    }
}
