using MagmaHeart.AI.Reasoning.Tests;
using MagmaHeart.Collections;
using System;

namespace MagmaHeart.AI.Reasoning
{
    internal class BasicStrategy : Strategy
    {
        private const float HEALTH_WEIGHT = 0.8f;
        private const float DISTANCE_WEIGHT = 0.1f;

        public BasicStrategy(int lookAhead, Func<StateSnapshot, AIUnit> playerTargetSelection, AIUnit player) : base(lookAhead, playerTargetSelection, player) { }

        public override float EvaluateState(StateSnapshot state)
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
            Position playerPosition = null;
            
            Func<Position, float> getDistancePoints = (ai) => {
                float distance = ai.Distance(playerPosition);
                if (distance == 0)
                    return 5;

                return 5 / distance;
            };

            foreach (var aiUnitWithProperties in state.StateProperties)
            {
                AIUnit unit = aiUnitWithProperties.Key;
                TypeMap<PropertySnapshot> unitProperties = aiUnitWithProperties.Value;
                Health health = unitProperties.Get<Health>();
                Position position = unitProperties.Get<Position>();
                IsAliveProperty isAlive = unitProperties.Get<IsAliveProperty>();

                if (unit.IsPlayer)
                {
                    playerIsAlive = isAlive;

                    if (isAlive)
                    {
                        playerHP = health.CurrentHealth;
                        playerPosition = position;
                    }
                    else
                    {
                        playerIsNotAlivePoints = 100;
                    }
                }
                else
                {
                    if (!isAlive)
                        ++aiNotAliveCount;
                    else
                        aiHP += health.CurrentHealth;
                }
            }

            if (playerIsAlive)
            {
                foreach (AIUnit aiUnit in state.StateProperties.Keys)
                {
                    if (aiUnit.IsPlayer)
                        continue;

                    IsAliveProperty isAlive = state.StateProperties[aiUnit].Get<IsAliveProperty>();
                    if (!isAlive)
                        continue;

                    Position position = state.StateProperties[aiUnit].Get<Position>();
                    distancePoints += getDistancePoints(position);
                }
            }

            return playerIsNotAlivePoints
                + aiIsNotAlivePoints * aiNotAliveCount
                + HEALTH_WEIGHT * (aiHP - playerHP)
                + DISTANCE_WEIGHT * distancePoints;
        }
    }
}
