using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Abilities;
using MagmaHeart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.AI
{
    [CreateAssetMenu(menuName = "AI/Strategy")]
    public class AggressiveStrategy : Strategy
    {
        [SerializeField] private float m_playerHealthWeight = 0.8f;
        [SerializeField] private float m_distanceWeight = 0.1f;
        [SerializeField] private float m_aiIsNotAlivePoints = -50;
        [SerializeField] private ParameterDatabase m_parameters;

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
            float playerHealthCoefficient = player.Health.MaxHealth / player.Health.CurrentHealth;

            if (player.IsDisabled())
                playerIsNotAlivePoints = 100;

            Func<Vector3, float> getDistancePoints = (ai) =>
            {
                float distance = Vector3.Distance(ai, player.TilePosition);
                if (distance == 0)
                    return 5;

                return 5 / distance;
            };

            foreach (AIUnitModel unit in allUnits)
            {
                if (unit.IsPlayer)
                    continue;

                if (unit.IsDisabled())
                {
                    ++aiNotAliveCount;
                }
                else
                {
                    aiHP += world.GetParameter(unit.Id, m_parameters.Health).CurrentValue;

                    if (!player.IsDisabled())
                    {
                        Vector3 position = world.GetEntityPosition(unit.Id);
                        distancePoints += getDistancePoints(position);
                    }
                }
            }

            return playerIsNotAlivePoints
                + m_aiIsNotAlivePoints * aiNotAliveCount
                + m_playerHealthWeight * playerHealthDifference
                + m_playerHealthWeight * playerHealthCoefficient
                + m_distanceWeight * distancePoints;
        }
    }
}
