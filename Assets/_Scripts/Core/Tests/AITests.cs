using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal class AITests
    {
        private CombatBoardState m_state;
        private BoardDimensions m_boardDimensions;
        private ActionDatabase m_actionDatabase;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_boardDimensions = new BoardDimensions(Vector2Int.zero, new Vector2Int(10, 10));
            Assembly assembly = FindAssembly("MagmaHeart.Core");
            m_actionDatabase = new ActionDatabase(assembly);
        }

        private Assembly FindAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);
        }

        [SetUp]
        public void SetUp()
        {
            Board board = BoardBuilder.CreateEmptyBoard(m_boardDimensions);
            Room room = new Room(null, null, null, board.Graph);
            m_state = new CombatBoardState(room);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementPlan_AggressiveStrategy_MovesTowardsPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(m_state)
                .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).At(0, 0)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(9, 9)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(best.ExecutedTasks.First().Action, Is.TypeOf<MovementAction>());
            Assert.That(best.Target, Is.EqualTo(scenario.Player));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackPlan_AggressiveStrategy_AttacksPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(m_state)
                .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(2));
            Assert.That(best.ExecutedTasks.All(task => task.Action.GetType() == typeof(AttackAction)));
            Assert.That(best.Target, Is.EqualTo(scenario.Player));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackPlan_EnemyWithLowHealthAndAggressiveStrategy_AttacksPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(m_state)
                .AddEntity().IsPlayer(false).WithHealth(1).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(2));
            Assert.That(best.ExecutedTasks.All(task => task.Action.GetType() == typeof(AttackAction)));
            Assert.That(best.Target, Is.EqualTo(scenario.Player));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task Null_PlayerSurroundedByWalls_EnemyDoesNothing(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(m_state)
                .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).SurroundWithWalls().At(4, 3)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            Assert.That(best, Is.Null);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementWithContinuousAttackPlan_OnePlayerTwoEnemiesWithLowHealth_EnemyMovesTowardsPlayerAndAttacksHim(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(m_state)
                .AddEntity().IsPlayer(false).WithHealth(1).WithActions(ActionPresets.MeleeAttacker).At(1, 1)
                .AddEntity().IsPlayer(true).WithHealth(4).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
                .AddEntity().IsPlayer(false).WithHealth(2).WithActions(ActionPresets.MeleeAttacker).At(3, 2)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            List<PlanTask> executedTasks = best.ExecutedTasks.ToList();
            Assert.That(executedTasks.Count, Is.EqualTo(2));
            Assert.That(executedTasks[0].Action, Is.TypeOf<MovementAction>());
            Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
            Assert.That(best.Target, Is.EqualTo(scenario.Player));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementWithContinuousAttackPlan_OnePlayerTwoEnemies_EnemyMovesTowardsPlayerAndAttacksHim(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(m_state)
                .AddEntity().IsPlayer(false).WithHealth(2).WithActions(ActionPresets.MeleeAttacker).At(0, 1)
                .AddEntity().IsPlayer(true).WithHealth(5).WithActions(ActionPresets.MeleeAttacker).At(2, 2)
                .AddEntity().IsPlayer(false).WithHealth(5).WithActions(ActionPresets.MeleeAttacker).At(2, 1)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            List<PlanTask> executedTasks = best.ExecutedTasks.ToList();
            Assert.That(executedTasks.Count, Is.EqualTo(3));
            Assert.That(executedTasks[0].Action, Is.TypeOf<MovementAction>());
            Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
            Assert.That(executedTasks[2].Action, Is.TypeOf<AttackAction>());
            Assert.That(best.Target, Is.EqualTo(scenario.Player));
        }
    }
}