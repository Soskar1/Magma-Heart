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
        public void ChooseBestMove_From3PossibleActions_ChoosesMoveAction()
        {
            BasicStrategy strategy = new BasicStrategy(1);
            TacticianAI tactician = new TacticianAI(m_player, m_enemySelection);
            tactician.CurrentStrategy = strategy;

            Entity enemy = new Entity(10, Vector2.zero, false);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { enemy, m_player };

            Action bestAction = tactician.ChooseBestMove(circularList);

            Assert.That(bestAction, Is.TypeOf<MoveAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ChooseBestMove_From3PossibleActions_ChoosesEngageAction(int depth)
        {
            BasicStrategy strategy = new BasicStrategy(depth);
            TacticianAI tactician = new TacticianAI(m_player, m_enemySelection);
            tactician.CurrentStrategy = strategy;

            Entity enemy = new Entity(10, new Vector2(5, 3), false);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { enemy, m_player };

            Action bestAction = tactician.ChooseBestMove(circularList);

            Assert.That(bestAction, Is.TypeOf<EngageAction>());
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChooseRunAwayAction()
        {
            BasicStrategy strategy = new BasicStrategy(2);
            TacticianAI tactician = new TacticianAI(m_player, m_enemySelection);
            tactician.CurrentStrategy = strategy;
            Entity enemy = new Entity(1, new Vector2(4, 5), false);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { enemy, m_player };

            Action bestAction = tactician.ChooseBestMove(circularList);

            Assert.That(bestAction, Is.TypeOf<RunAwayAction>());
        }
    }
}
