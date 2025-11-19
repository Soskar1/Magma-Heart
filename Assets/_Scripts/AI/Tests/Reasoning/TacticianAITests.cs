using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class TacticianAITests
    {
        private Entity m_player;
        private Board m_board;
        private DummyActualGameState m_state;

        internal class DummyActualGameState : ActualBoardState
        {
            public DummyActualGameState(Board board) : base(board) { }
            public override T GetProperty<T>(AIUnit unit) => throw new NotImplementedException();
        }

        private Func<int, Vector2, bool, Board, Entity> Entity = (health, position, isPlayer, board) =>
        {
            Entity entity = new Entity(health, position, isPlayer);
            entity.PossibleActions.Add(new AttackAction(entity, 4));
            entity.PossibleActions.Add(new MoveAction(entity, 3));
            entity.PossibleActions.Add(new EngageAction(entity, 4, 1));
            entity.PossibleActions.Add(new RunAwayAction(entity, 3));

            board.AddUnit(position, entity);
            return entity;
        };

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = -10; i < 10; ++i)
                for (int j = -10; j < 10; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            m_board = new Board(graph);
            m_player = Entity(10, new Vector2(5, 5), true, m_board);
            m_state = new DummyActualGameState(m_board);
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChoosesMoveAction()
        {
            BasicStrategy strategy = new BasicStrategy(1, m_player);
            TacticianAI tactician = new TacticianAI(strategy);
            Entity enemy = Entity(10, Vector2.zero, false, m_board);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { enemy, m_player };

            BestAction bestAction = tactician.ChooseBestMove(circularList, m_state);

            Assert.That(bestAction.Action, Is.TypeOf<MoveAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ChooseBestMove_From3PossibleActions_ChoosesEngageAction(int depth)
        {
            BasicStrategy strategy = new BasicStrategy(depth, m_player);
            TacticianAI tactician = new TacticianAI(strategy);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { Entity(10, new Vector2(5, 3), false, m_board), m_player };

            BestAction bestAction = tactician.ChooseBestMove(circularList, m_state);

            Assert.That(bestAction.Action, Is.TypeOf<EngageAction>());
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChooseRunAwayAction()
        {
            BasicStrategy strategy = new BasicStrategy(2, m_player);
            TacticianAI tactician = new TacticianAI(strategy);
            CircularList<AIUnit> circularList = new CircularList<AIUnit>() { Entity(1, new Vector2(4, 5), false, m_board), m_player };

            BestAction bestAction = tactician.ChooseBestMove(circularList, m_state);

            Assert.That(bestAction.Action, Is.TypeOf<RunAwayAction>());
        }
    }
}
