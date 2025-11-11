using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Entities.Properties;
using System;

namespace MagmaHeart.Core.AI
{
    public class AggressiveStrategy : Strategy
    {
        private const float HEALTH_WEIGHT = 0.8f;
        private const float DISTANCE_WEIGHT = 0.1f;

        public AggressiveStrategy(int lookAhead, Func<StateSnapshot, AIUnit> playerTargetSelection, AIUnit player) : base(lookAhead, playerTargetSelection, player) { }

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
            PositionPropertySnapshot playerPosition = state.GetProperty<PositionPropertySnapshot>(Player);

            Func<PositionPropertySnapshot, float> getDistancePoints = (ai) => {
                float distance = ai.ManhattanDistance(playerPosition);
                if (distance == 0)
                    return 5;

                return 5 / distance;
            };

            foreach (var unit in state.GetAllUnits())
            {
                HealthPropertySnapshot health = state.GetProperty<HealthPropertySnapshot>(unit);
                PositionPropertySnapshot position = state.GetProperty<PositionPropertySnapshot>(unit);
                IsAlivePropertySnapshot isAlive = state.GetProperty<IsAlivePropertySnapshot>(unit);

                if (unit.IsPlayer)
                {
                    playerIsAlive = isAlive;

                    if (isAlive)
                        playerHP = health.CurrentHealth;
                    else
                        playerIsNotAlivePoints = 100;
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

                    IsAlivePropertySnapshot isAlive = state.GetProperty<IsAlivePropertySnapshot>(aiUnit);
                    if (!isAlive)
                        continue;

                    PositionPropertySnapshot position = state.GetProperty<PositionPropertySnapshot>(aiUnit);
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
