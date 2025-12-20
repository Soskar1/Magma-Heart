using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AIEngineTests
    {
        private EmptyTurnController m_player;
        private Board m_board;
        private DummyActualGameState m_state;
        private Assembly m_assembly;

        internal class EmptyTurnController : TurnContext
        {
            public EmptyTurnController(AIUnitModel owner) : base(owner) { }

            public override IEnumerable<StateChange> ProduceStartTurnChanges() => new List<StateChange>();
        }

        internal class DummyActualGameState : ActualBoardState
        {
            public DummyActualGameState(Board board) : base(board) { }
            public override T GetProperty<T>(AIUnitModel unit) => throw new NotImplementedException();
        }

        private Func<int, Vector2, bool, Board, Entity> Entity = (health, position, isPlayer, board) =>
        {
            Entity entity = new Entity(health, position, isPlayer);
            AttackActionData attackData = new AttackActionData(4);
            MoveActionData moveData = new MoveActionData(3);
            EngageActionData engageData = new EngageActionData(4, 1);
            RunAwayActionData runAwayData = new RunAwayActionData(3);

            entity.PossibleActions.Add(attackData.GetDefinition());
            entity.PossibleActions.Add(moveData.GetDefinition());
            entity.PossibleActions.Add(engageData.GetDefinition());
            entity.PossibleActions.Add(runAwayData.GetDefinition());

            board.AddUnit(position, entity);
            return entity;
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_assembly = Assembly.GetExecutingAssembly();
        }

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = -10; i < 10; ++i)
                for (int j = -10; j < 10; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            m_board = new Board(graph);
            m_player = new EmptyTurnController(Entity(10, new Vector2(5, 5), true, m_board));
            m_state = new DummyActualGameState(m_board);
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChoosesMoveAction()
        {
            BasicStrategy strategy = new BasicStrategy(m_player.Model);
            AIEngine engine = new AIEngine(strategy, new ActionDatabase(m_assembly), 1);
            EmptyTurnController enemy = new EmptyTurnController(Entity(10, Vector2.zero, false, m_board));
            CircularList<TurnContext> turnOrder = new CircularList<TurnContext>() { enemy, m_player };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.Plan.Task.Action, Is.TypeOf<MoveAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ChooseBestMove_From3PossibleActions_ChoosesEngageAction(int depth)
        {
            BasicStrategy strategy = new BasicStrategy(m_player.Model);
            AIEngine engine = new AIEngine(strategy, new ActionDatabase(m_assembly), depth);
            EmptyTurnController enemy = new EmptyTurnController(Entity(10, new Vector2(5, 3), false, m_board));
            CircularList<TurnContext> turnOrder = new CircularList<TurnContext>() { enemy, m_player };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.Plan.Task.Action, Is.TypeOf<EngageAction>());
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChooseRunAwayAction()
        {
            BasicStrategy strategy = new BasicStrategy(m_player.Model);
            AIEngine engine = new AIEngine(strategy, new ActionDatabase(m_assembly), 2);
            EmptyTurnController enemy = new EmptyTurnController(Entity(1, new Vector2(4, 5), false, m_board));
            CircularList<TurnContext> turnOrder = new CircularList<TurnContext>() { enemy, m_player };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.Plan.Task.Action, Is.TypeOf<RunAwayAction>());
        }
    }
}
