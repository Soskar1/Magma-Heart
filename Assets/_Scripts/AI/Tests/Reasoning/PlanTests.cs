using MagmaHeart.AI.Actions;
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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_attackData = new AttackActionData(4);
            MoveActionData moveData = new MoveActionData(1);
            EngageActionData engageData = new EngageActionData(4, 1);
            RunAwayActionData runAwayData = new RunAwayActionData(3);

            ActionDefinition attackDefinition = m_attackData.GetDefinition();
            m_attackTask = new PlanTask(Database.Get(attackDefinition.ActionType), attackDefinition);

            ActionDefinition moveDefinition = moveData.GetDefinition();
            m_moveTask = new PlanTask(Database.Get(moveDefinition.ActionType), moveDefinition);
        }

        [Test]
        public void Constructor_OnePlanTask_CreatesPlanWithOneTask()
        {
            Plan plan = new Plan(new List<PlanTask>() { m_attackTask });
            
            Assert.That(plan.Tasks.Count(), Is.EqualTo(1));
            Assert.That(plan.Tasks.First(), Is.EqualTo(m_attackTask));
        }

        [Test]
        public void Constructor_TwoPlanTask_CreatesPlanWithTwoTask()
        {
            Plan plan = new Plan(new List<PlanTask> { m_attackTask, m_moveTask });

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
            SimulatedBoardState simulation = new SimulatedBoardState(Board);
            List<PlanTask> planTasks = new List<PlanTask>();
            for (int i = 0; i < executionTimes; ++i)
                planTasks.Add(m_attackTask);
            Plan plan = new Plan(planTasks);

            bool executed = plan.TryExecute(simulation, enemy);

            Assert.That(executed, Is.True);
            Health health = simulation.GetProperty<Health>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(initialHealth - m_attackData.Damage * executionTimes));
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
            SimulatedBoardState simulation = new SimulatedBoardState(Board);
            List<PlanTask> planTasks = new List<PlanTask>();
            for (int i = 0; i < executionTimes; ++i)
                planTasks.Add(m_attackTask);
            Plan plan = new Plan(planTasks);
            plan.TryExecute(simulation, enemy);

            plan.Undo(simulation);

            Health health = simulation.GetProperty<Health>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(initialHealth));
        }

        [Test]
        public void TryExecute_FailExecutionOnFirstTask_ReturnsFalse()
        {
            int initialHealth = 10;
            Entity player = CreateEntity(initialHealth, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(initialHealth, new Vector2(3, 5), false);
            SimulatedBoardState simulation = new SimulatedBoardState(Board);
            List<PlanTask> planTasks = new List<PlanTask>() { m_attackTask };
            Plan plan = new Plan(planTasks);
            
            bool executed = plan.TryExecute(simulation, enemy);

            Assert.That(executed, Is.False);
            Health health = simulation.GetProperty<Health>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(initialHealth));
        }

        [Test]
        public void TryExecute_PlanFailsOnThirdTask_AutomaticallyCallsUndo()
        {
            int initialHealth = 10;
            Vector2 initialEnemyPosition = new Vector2(0, 5);
            Entity player = CreateEntity(initialHealth, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(initialHealth, initialEnemyPosition, false);
            SimulatedBoardState simulation = new SimulatedBoardState(Board);
            List<PlanTask> planTasks = new List<PlanTask>() {
                m_moveTask, m_moveTask, m_attackTask
            };
            Plan plan = new Plan(planTasks);

            bool executed = plan.TryExecute(simulation, enemy);

            Assert.That(executed, Is.False);
            Health health = simulation.GetProperty<Health>(player);
            Assert.That(health.CurrentHealth, Is.EqualTo(initialHealth));
            Position position = simulation.GetProperty<Position>(enemy);
            Assert.That(position.CurrentPosition, Is.EqualTo(initialEnemyPosition));
        }
    }
}
