using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities.Properties;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.AI
{
    public class AggressiveStrategy : Strategy
    {
        private const float PLAYER_HEALTH_WEIGHT = 0.8f;
        private const float DISTANCE_WEIGHT = 0.1f;
        private const float AI_IS_NOT_ALIVE_POINTS = -50;

        public AggressiveStrategy(AIUnitModel player) : base(player) {
            Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
                new PlanTaskDefinition(typeof(MovementAction))
            }, new EnemyTargetSelector()));

            Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
                new PlanTaskDefinition(typeof(AttackAction), true),
            }, new EnemyTargetSelector()));

            Plans.Add(new PlanDefinition(new List<PlanTaskDefinition>() {
                new PlanTaskDefinition(typeof(MovementAction)),
                new PlanTaskDefinition(typeof(AttackAction), true)
            }, new EnemyTargetSelector()));
        }

        public override float EvaluateState(SimulatedBoardState state)
        {
            // !IS_ALIVE == -50 if AI
            // !IS_ALIVE == 100 if PLAYER
            // 1 * (PLAYER_IS_NOT_ALIVE + AI_IS_NOT_ALIVE) + w1 * PLAYER_HP_DIFF + w2 * (5 / AI_DISTANCE_TO_PLAYER)

            float aiHP = 0;
            float distancePoints = 0;
            float playerIsNotAlivePoints = 0;
            int aiNotAliveCount = 0;

            PositionPropertySnapshot playerPosition = state.GetProperty<PositionPropertySnapshot>(Player);
            bool playerIsAlive = state.GetProperty<IsAlivePropertySnapshot>(Player);
            HealthPropertySnapshot playerHealth = state.GetProperty<HealthPropertySnapshot>(Player);
            float playerHealthDifference = playerHealth.MaxHealth - playerHealth.CurrentHealth;

            if (!playerIsAlive)
                playerIsNotAlivePoints = 100;

            Func<PositionPropertySnapshot, float> getDistancePoints = (ai) =>
            {
                float distance = ai.ManhattanDistance(playerPosition);
                if (distance == 0)
                    return 5;

                return 5 / distance;
            };

            foreach (var unit in state.Board.GetUnits())
            {
                if (unit.IsPlayer)
                    continue;

                HealthPropertySnapshot health = state.GetProperty<HealthPropertySnapshot>(unit);
                PositionPropertySnapshot position = state.GetProperty<PositionPropertySnapshot>(unit);
                IsAlivePropertySnapshot isAlive = state.GetProperty<IsAlivePropertySnapshot>(unit);

                if (!isAlive)
                {
                    ++aiNotAliveCount;
                }
                else
                {
                    aiHP += health.CurrentHealth;

                    if (playerIsAlive)
                        distancePoints += getDistancePoints(position);
                }
            }

            return playerIsNotAlivePoints
                + AI_IS_NOT_ALIVE_POINTS * aiNotAliveCount
                + PLAYER_HEALTH_WEIGHT * playerHealthDifference
                + DISTANCE_WEIGHT * distancePoints;
        }
    }
}
