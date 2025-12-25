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
    internal class AIEngineTests : ReasoningTests
    {
        private EmptyTurnController m_player;
        private DummyActualGameState m_state;

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

        [SetUp]
        public void SetUp()
        {
            m_player = new EmptyTurnController(CreateEntity(10, new Vector2(5, 5), true));
            m_state = new DummyActualGameState(Board);
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChoosesMoveAction()
        {
            BasicStrategy strategy = new BasicStrategy();
            AIEngine engine = new AIEngine(strategy, Database, 1);
            EmptyTurnController enemy = new EmptyTurnController(CreateEntity(10, Vector2.zero, false));
            CircularList<TurnContext> turnOrder = new CircularList<TurnContext>() { enemy, m_player };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(bestPlan.ExecutedTasks.First().Action, Is.TypeOf<MoveAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void ChooseBestMove_From3PossibleActions_ChoosesEngageAction(int depth)
        {
            BasicStrategy strategy = new BasicStrategy();
            AIEngine engine = new AIEngine(strategy, Database, depth);
            EmptyTurnController enemy = new EmptyTurnController(CreateEntity(10, new Vector2(5, 3), false));
            CircularList<TurnContext> turnOrder = new CircularList<TurnContext>() { enemy, m_player };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(bestPlan.ExecutedTasks.First().Action, Is.TypeOf<EngageAction>());
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChooseRunAwayAction()
        {
            BasicStrategy strategy = new BasicStrategy();
            AIEngine engine = new AIEngine(strategy, Database, 2);
            EmptyTurnController enemy = new EmptyTurnController(CreateEntity(1, new Vector2(4, 5), false));
            CircularList<TurnContext> turnOrder = new CircularList<TurnContext>() { enemy, m_player };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(bestPlan.ExecutedTasks.First().Action, Is.TypeOf<RunAwayAction>());
        }
    }
}
