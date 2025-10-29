using MagmaHeart.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class TacticianAITests
    {
        private Entity m_player;
        private Func<StateSnapshot, AIUnit> m_enemySelection;

        [SetUp]
        public void SetUp()
        {
            m_player = new Entity(10, new Vector2(5, 5), true);
            m_enemySelection = (state) =>
            {
                AIUnit nearestUnit = null;
                float minDistance = float.MaxValue;

                List<AIUnit> allUnits = state.GetAllUnits();
                foreach (AIUnit unit in allUnits)
                {
                    if (unit.IsPlayer)
                        continue;

                    IsAliveProperty isAlive = state.GetProperty<IsAliveProperty>(unit);
                    if (!isAlive)
                        continue;

                    Position unitPosition = state.GetProperty<Position>(unit);

                    float distance = unitPosition.Distance(m_player.Position);
                    if (distance < minDistance)
                    {
                        nearestUnit = unit;
                        minDistance = distance;
                    }
                }

                return nearestUnit;
            };
        }

        [Test]
        public void TacticianAI_ChooseBestMoveDepth1_ConsidersOnlyMoveAction()
        {
            TacticianAI tactician = new TacticianAI(1, m_player, m_enemySelection);
            Entity enemy = new Entity(5, Vector2.zero, false);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { enemy, m_player };

            IAction bestAction = tactician.ChooseBestMove(circularList);

            Assert.That(bestAction, Is.TypeOf<MoveAction>());
        }

        [Test]
        public void TacticianAI_ChooseBestMoveFrom2PossibleActionsDepth1_ChoosesEngageAction()
        {
            TacticianAI tactician = new TacticianAI(1, m_player, m_enemySelection);
            Entity enemy = new Entity(5, new Vector2(5, 3), false);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { enemy, m_player };

            IAction bestAction = tactician.ChooseBestMove(circularList);

            Assert.That(bestAction, Is.TypeOf<EngageAction>());
        }
    }
}
