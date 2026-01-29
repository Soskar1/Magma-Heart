using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AIEngineTests : ReasoningTests
    {
        private EmptyTurnContext m_turnContext;
        private DummyActualGameState m_state;
        private Entity m_player;

        internal class EmptyTurnContext : TurnContext
        {
            public override IEnumerable<StateChange> ProduceStartTurnChanges(AIUnitModel model) => new List<StateChange>();
        }

        internal class DummyActualGameState : ActualBoardState
        {
            public DummyActualGameState(Board board) : base(board) { }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_turnContext = new EmptyTurnContext();
        }

        [SetUp]
        public void Initialize()
        {
            m_player = CreateEntity(10, new Vector2(5, 5), true);
            m_state = new DummyActualGameState(Board);
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChoosesMoveAction()
        {
            BasicStrategy strategy = new BasicStrategy();
            AIEngine engine = new AIEngine(strategy, Database, 1, m_turnContext);
            Entity enemy = CreateEntity(10, Vector2.zero, false);
            CircularList<int> turnOrder = new CircularList<int>() { enemy.Id, m_player.Id };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(bestPlan.ExecutedTasks.First().Action, Is.TypeOf<MoveAction>());
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChooseRunAwayAction()
        {
            BasicStrategy strategy = new BasicStrategy();
            AIEngine engine = new AIEngine(strategy, Database, 2, m_turnContext);
            Entity enemy = CreateEntity(1, new Vector2(4, 5), false);
            CircularList<int> turnOrder = new CircularList<int>() { enemy.Id, m_player.Id };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, m_state);

            Assert.That(bestPlan.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(bestPlan.ExecutedTasks.First().Action, Is.TypeOf<RunAwayAction>());
        }
    }
}
