using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class PlanTests : ReasoningTests
    {
        private PlanTask m_attackTask;
        private PlanTask m_moveTask;

        private AttackActionData m_attackData;

        private CommandRunner m_commandRunner;

        [OneTimeSetUp]
        public void InitializeTasks()
        {
            m_attackData = new AttackActionData(4);
            MoveActionData moveData = new MoveActionData(1);
            RunAwayActionData runAwayData = new RunAwayActionData(3);

            ActionDefinition attackDefinition = m_attackData.GetDefinition();
            m_attackTask = new PlanTask(Database.Get(attackDefinition.ActionType), attackDefinition);

            ActionDefinition moveDefinition = moveData.GetDefinition();
            m_moveTask = new PlanTask(Database.Get(moveDefinition.ActionType), moveDefinition);

            
        }

        [SetUp]
        public void Setup()
        {
            m_commandRunner = new CommandRunner();
        }

        [Test]
        public void Constructor_OnePlanTask_CreatesPlanWithOneTask()
        {
            Plan plan = new Plan(new List<PlanTask>() { m_attackTask }, m_commandRunner);
            
            Assert.That(plan.Tasks.Count(), Is.EqualTo(1));
            Assert.That(plan.Tasks.First(), Is.EqualTo(m_attackTask));
        }

        [Test]
        public void Constructor_TwoPlanTask_CreatesPlanWithTwoTask()
        {
            Plan plan = new Plan(new List<PlanTask> { m_attackTask, m_moveTask }, m_commandRunner);

            Assert.That(plan.Tasks.Count(), Is.EqualTo(2));
            Assert.That(plan.Tasks.First(), Is.EqualTo(m_attackTask));
            Assert.That(plan.Tasks.Last(), Is.EqualTo(m_moveTask));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TryExecute_ExecutesSuccessfullyMultipleTimes(int executionTimes)
        {
            int initialHealth = 10;
            Entity player = CreateEntity(initialHealth, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(initialHealth, new Vector2(4, 5), false);
            Board simulation = Board.DeepCopy();
            List<PlanTask> planTasks = new List<PlanTask>();
            for (int i = 0; i < executionTimes; ++i)
                planTasks.Add(m_attackTask);
            Plan plan = new Plan(planTasks, m_commandRunner);

            bool executed = plan.TryExecute(simulation, enemy);

            Assert.That(executed, Is.True);
            simulation.TryGetUnit(player.id, out Entity simulatedPlayer);
            Assert.That(simulatedPlayer.CurrentHealth, Is.EqualTo(initialHealth - m_attackData.Damage * executionTimes));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Undo_UndosSuccessfullyMultipleTasks(int executionTimes)
        {
            int initialHealth = 10;
            Entity player = CreateEntity(initialHealth, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(initialHealth, new Vector2(4, 5), false);
            Board simulation = Board.DeepCopy();
            List<PlanTask> planTasks = new List<PlanTask>();
            for (int i = 0; i < executionTimes; ++i)
                planTasks.Add(m_attackTask);
            Plan plan = new Plan(planTasks, m_commandRunner);
            plan.TryExecute(simulation, enemy);

            plan.Undo(simulation);

            simulation.TryGetUnit(player.id, out Entity simulatedPlayer);
            Assert.That(simulatedPlayer.CurrentHealth, Is.EqualTo(initialHealth));
        }

        [Test]
        public void TryExecute_FailExecutionOnFirstTask_ReturnsFalse()
        {
            int initialHealth = 10;
            Entity player = CreateEntity(initialHealth, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(initialHealth, new Vector2(3, 5), false);
            Board simulation = Board.DeepCopy();
            List<PlanTask> planTasks = new List<PlanTask>() { m_attackTask };
            Plan plan = new Plan(planTasks, m_commandRunner);
            
            bool executed = plan.TryExecute(simulation, enemy);

            Assert.That(executed, Is.False);
            simulation.TryGetUnit(player.id, out Entity simulatedPlayer);
            Assert.That(simulatedPlayer.CurrentHealth, Is.EqualTo(initialHealth));
        }

        [Test]
        public void TryExecute_PlanFailsOnThirdTask_AutomaticallyCallsUndo()
        {
            int initialHealth = 10;
            Vector2 initialEnemyPosition = new Vector2(0, 5);
            Entity player = CreateEntity(initialHealth, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(initialHealth, initialEnemyPosition, false);
            Board simulation = Board.DeepCopy();
            List<PlanTask> planTasks = new List<PlanTask>() {
                m_moveTask, m_moveTask, m_attackTask
            };
            Plan plan = new Plan(planTasks, m_commandRunner);

            bool executed = plan.TryExecute(simulation, enemy);

            Assert.That(executed, Is.False);

            simulation.TryGetUnit(player.id, out Entity simulatedPlayer);
            Assert.That(simulatedPlayer.CurrentHealth, Is.EqualTo(initialHealth));

            simulation.TryGetUnit(enemy.id, out Entity simulatedEnemy);
            Assert.That(simulatedEnemy.Position, Is.EqualTo(initialEnemyPosition));
        }
    }
}
