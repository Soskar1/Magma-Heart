using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.Core.BoardStateSystem.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Tests
{
    internal class AITests : CoreTests
    {
        private ActionDatabase m_actionDatabase;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Assembly assembly = FindAssembly("MagmaHeart.Core");
            m_actionDatabase = new ActionDatabase(assembly);
        }

        private Assembly FindAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementPlan_AggressiveStrategy_MovesTowardsPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).At(0, 0)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(9, 9)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(1));
            Assert.That(best.ExecutedTasks.First().Action, Is.TypeOf<MovementAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackPlan_AggressiveStrategy_AttacksPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(2));
            Assert.That(best.ExecutedTasks.All(task => task.Action.GetType() == typeof(AttackAction)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task AttackPlan_EnemyWithLowHealthAndAggressiveStrategy_AttacksPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(false).WithHealth(1).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(2));
            Assert.That(best.ExecutedTasks.All(task => task.Action.GetType() == typeof(AttackAction)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task Null_PlayerSurroundedByWalls_EnemyDoesNothing(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).At(4, 3)
                .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
                .ModifyBoard().SurroundWithWalls(4, 3).Bake()
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
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(false).WithHealth(1).WithActions(ActionPresets.MeleeAttacker).At(1, 1)
                .AddEntity().IsPlayer(true).WithHealth(4).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
                .AddEntity().IsPlayer(false).WithHealth(2).WithActions(ActionPresets.MeleeAttacker).At(3, 2)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            List<PlanTask> executedTasks = best.ExecutedTasks.ToList();
            Assert.That(executedTasks.Count, Is.EqualTo(2));
            Assert.That(executedTasks[0].Action, Is.TypeOf<MovementAction>());
            Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task MovementWithContinuousAttackPlan_OnePlayerTwoEnemies_EnemyMovesTowardsPlayerAndAttacksHim(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(State)
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
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task RangedAttack_WallPlacedBetweenEnemyAndPlayer_EnemyDoNotUseRangedAttackAsAFirstAction(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(false).WithHealth(2).WithActions(ActionPresets.RangedAttacker).At(2, 0)
                .AddEntity().IsPlayer(true).WithHealth(5).WithActions(ActionPresets.MeleeAttacker).At(2, 2)
                .ModifyBoard().PlaceWallAt(2, 1).Bake()
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            List<PlanTask> executedTasks = best.ExecutedTasks.ToList();
            Assert.That(executedTasks.Count, Is.EqualTo(2));
            Assert.That(executedTasks[0].Action, Is.TypeOf<MovementAction>());
            Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task RangedAttack_EnemyIsFarAwayFromPlayer_EnemyUseRangedAttack(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(State)
                .AddEntity().IsPlayer(false).WithHealth(2).WithActions(ActionPresets.RangedAttacker).At(2, 0)
                .AddEntity().IsPlayer(true).WithHealth(5).WithActions(ActionPresets.MeleeAttacker).At(2, 4)
                .Build();

            BestPlan best = await scenario.RunAI(depth, m_actionDatabase);

            List<PlanTask> executedTasks = best.ExecutedTasks.ToList();
            Assert.That(executedTasks.Count, Is.EqualTo(2));
            Assert.That(executedTasks[0].Action, Is.TypeOf<AttackAction>());
            Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
        }
    }
}