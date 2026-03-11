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
        [SerializeField] private float m_aiIsNotAlivePoints = -50;
        [SerializeField] private ParameterDatabase m_parameters;
        [SerializeField] private float m_playerIsNotAlivePoints = 100;

        public override float EvaluateState(IBoardGameWorld world)
        {
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
                    ++aiNotAliveCount;
            }

            return m_aiIsNotAlivePoints * aiNotAliveCount
                + m_playerHealthWeight * playerHealthDifference
                + m_playerHealthWeight * playerHealthCoefficient
                + (player.IsDisabled() ? m_playerIsNotAlivePoints : 0);
        }
    }
}
