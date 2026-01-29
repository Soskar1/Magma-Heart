using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Execution;
using MagmaHeart.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class AIEngineTests : ReasoningTests
    {
        private CommandFactory m_commandFactory;
        private Entity m_player;

        internal class CommandFactory : IStartOfTurnCommandFactory
        {
            public IEnumerable<IBoardCommand> BuildStartOfTurnCommands(Board board, AIUnitModel unit) => new List<IBoardCommand>();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp() => m_commandFactory = new CommandFactory();

        [SetUp]
        public void Initialize()
        {
            m_player = CreateEntity(10, new Vector2(5, 5), true);
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChoosesMoveAction()
        {
            BasicStrategy strategy = new BasicStrategy();
            AIEngine engine = new AIEngine(strategy, Database, 1, m_commandFactory);
            Entity enemy = CreateEntity(10, Vector2.zero, false);
            CircularList<int> turnOrder = new CircularList<int>() { enemy.Id, m_player.Id };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, Board);

            Assert.That(bestPlan.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(bestPlan.ExecutedTasks.First().Action, Is.TypeOf<MoveAction>());
        }

        [Test]
        public void ChooseBestMove_From3PossibleActions_ChooseRunAwayAction()
        {
            BasicStrategy strategy = new BasicStrategy();
            AIEngine engine = new AIEngine(strategy, Database, 2, m_commandFactory);
            Entity enemy = CreateEntity(1, new Vector2(4, 5), false);
            CircularList<int> turnOrder = new CircularList<int>() { enemy.Id, m_player.Id };

            BestPlan bestPlan = engine.ChooseBestMove(turnOrder, Board);

            Assert.That(bestPlan.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(bestPlan.ExecutedTasks.First().Action, Is.TypeOf<RunAwayAction>());
        }
    }
}
