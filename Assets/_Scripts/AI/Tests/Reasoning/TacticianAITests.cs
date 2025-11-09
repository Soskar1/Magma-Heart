using MagmaHeart.AI.Boards;
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
        private Board m_board;

        private Func<int, Vector2, bool, Entity> Entity = (health, position, isPlayer) =>
        {
            Entity entity = new Entity(health, position, isPlayer);
            entity.PossibleActions.Add(new AttackAction(entity, 4));
            entity.PossibleActions.Add(new MoveAction(entity, 3));
            entity.PossibleActions.Add(new EngageAction(entity, 4, 1));
            entity.PossibleActions.Add(new RunAwayAction(entity, 3));
            return entity;
        };

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 5; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            m_board = new Board(graph);
            m_player = Entity(10, new Vector2(5, 5), true);
            m_enemySelection = (state) =>
            {
                AIUnit nearestUnit = null;
                float minDistance = float.MaxValue;

                List<AIUnit> allUnits = state.GetAllUnits();
                foreach (AIUnit unit in allUnits)
                {
                    if (unit.IsPlayer)
                        continue;

                    IsAlivePropertySnapshot isAlive = state.GetProperty<IsAlivePropertySnapshot>(unit);
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
            BasicStrategy strategy = new BasicStrategy(1, m_enemySelection, m_player);
            TacticianAI tactician = new TacticianAI(strategy);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { Entity(10, Vector2.zero, false), m_player };

            Action bestAction = tactician.ChooseBestMove(circularList, m_board);

            Assert.That(bestAction, Is.TypeOf<MoveAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ChooseBestMove_From3PossibleActions_ChoosesEngageAction(int depth)
        {
            BasicStrategy strategy = new BasicStrategy(depth, m_enemySelection, m_player);
            TacticianAI tactician = new TacticianAI(strategy);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { Entity(10, new Vector2(5, 3), false), m_player };

            Action bestAction = tactician.ChooseBestMove(circularList, m_board);

            Assert.That(bestAction, Is.TypeOf<EngageAction>());
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChooseRunAwayAction()
        {
            BasicStrategy strategy = new BasicStrategy(2, m_enemySelection, m_player);
            TacticianAI tactician = new TacticianAI(strategy);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { Entity(1, new Vector2(4, 5), false), m_player };

            Action bestAction = tactician.ChooseBestMove(circularList, m_board);

            Assert.That(bestAction, Is.TypeOf<RunAwayAction>());
        }
    }
}
