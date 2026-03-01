using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MagmaHeart.Abilities;
using MagmaHeart.Core.Abilities.Effects;
using NUnit.Framework;

namespace MagmaHeart.Core.Tests
{
    public class AITests : CoreTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void MovementPlan_AggressiveStrategy_MovesTowardsPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(World)
                .AddEntity().IsPlayer(false).WithData(EntityDatabase.SkeletonWarrior).At(0, 0)
                .AddEntity().IsPlayer(true).WithData(EntityDatabase.Warrior).At(9, 9)
                .Build();

            IList<AbilityPlan> best = scenario.RunAI(depth, TestDatabase).ToList();

            Assert.That(best.Count, Is.EqualTo(1));
            Assert.That(best.First().Effects, Has.Some.InstanceOf<MoveEffect>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void AttackPlan_AggressiveStrategy_AttacksPlayer(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(Board)
            //    .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
            //    .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
            //    .Build();

            //BestPlan best = scenario.RunAI(depth);

            //Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(2));
            throw new System.NotImplementedException("FIX THIS");
            //Assert.That(best.ExecutedTasks.All(task => task.Action.GetType() == typeof(AttackAction)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void AttackPlan_EnemyWithLowHealthAndAggressiveStrategy_AttacksPlayer(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(Board)
            //    .AddEntity().IsPlayer(false).WithHealth(1).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
            //    .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
            //    .Build();

            //BestPlan best = scenario.RunAI(depth);

            //Assert.That(best.ExecutedTasks.Count(), Is.EqualTo(2));
            // throw new System.NotImplementedException("FIX THIS");
            //Assert.That(best.ExecutedTasks.All(task => task.Action.GetType() == typeof(AttackAction)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Null_PlayerSurroundedByWalls_EnemyDoesNothing(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(Board)
            //    .AddEntity().IsPlayer(false).WithActions(ActionPresets.MeleeAttacker).At(4, 3)
            //    .AddEntity().IsPlayer(true).WithActions(ActionPresets.MeleeAttacker).At(2, 3)
            //    .ModifyBoard().SurroundWithWalls(4, 3).Bake()
            //    .Build();

            //BestPlan best = scenario.RunAI(depth);

            //Assert.That(best, Is.Null);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void MovementWithContinuousAttackPlan_OnePlayerTwoEnemiesWithLowHealth_EnemyMovesTowardsPlayerAndAttacksHim(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(Board)
            //    .AddEntity().IsPlayer(false).WithHealth(1).WithActions(ActionPresets.MeleeAttacker).At(1, 1)
            //    .AddEntity().IsPlayer(true).WithHealth(4).WithActions(ActionPresets.MeleeAttacker).At(3, 3)
            //    .AddEntity().IsPlayer(false).WithHealth(2).WithActions(ActionPresets.MeleeAttacker).At(3, 2)
            //    .Build();

            //BestPlan best = scenario.RunAI(depth);
            // throw new System.NotImplementedException("FIX THIS");
            //List<ExecutedTask> executedTasks = best.ExecutedTasks.ToList();
            //Assert.That(executedTasks.Count, Is.EqualTo(2));
            //Assert.That(executedTasks[0].Action, Is.TypeOf<MovementAction>());
            //Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void MovementWithContinuousAttackPlan_OnePlayerTwoEnemies_EnemyMovesTowardsPlayerAndAttacksHim(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(Board)
            //    .AddEntity().IsPlayer(false).WithHealth(2).WithEnergy(5).WithActions(ActionPresets.MeleeAttacker).At(0, 1)
            //    .AddEntity().IsPlayer(true).WithHealth(5).WithEnergy(5).WithActions(ActionPresets.MeleeAttacker).At(2, 2)
            //    .AddEntity().IsPlayer(false).WithHealth(5).WithEnergy(5).WithActions(ActionPresets.MeleeAttacker).At(2, 1)
            //    .Build();

            //BestPlan best = scenario.RunAI(depth);
            // throw new System.NotImplementedException("FIX THIS");
            //List<ExecutedTask> executedTasks = best.ExecutedTasks.ToList();
            //Assert.That(executedTasks.Count, Is.EqualTo(3));
            //Assert.That(executedTasks[0].Action, Is.TypeOf<MovementAction>());
            //Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
            //Assert.That(executedTasks[2].Action, Is.TypeOf<AttackAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void RangedAttack_WallPlacedBetweenEnemyAndPlayer_EnemyDoNotUseRangedAttackAsAFirstAction(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(Board)
            //    .AddEntity().IsPlayer(false).WithHealth(2).WithEnergy(5).WithActions(ActionPresets.RangedAttacker).At(2, 0)
            //    .AddEntity().IsPlayer(true).WithHealth(5).WithEnergy(5).WithActions(ActionPresets.MeleeAttacker).At(2, 2)
            //    .ModifyBoard().PlaceWallAt(2, 1).Bake()
            //    .Build();

            //BestPlan best = scenario.RunAI(depth);
            //throw new System.NotImplementedException("FIX THIS");
            //List<ExecutedTask> executedTasks = best.ExecutedTasks.ToList();
            //Assert.That(executedTasks.Count, Is.EqualTo(2));
            //Assert.That(executedTasks[0].Action, Is.TypeOf<MovementAction>());
            //Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void RangedAttack_EnemyIsFarAwayFromPlayer_EnemyUseRangedAttack(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(Board)
            //    .AddEntity().IsPlayer(false).WithHealth(2).WithEnergy(5).WithActions(ActionPresets.RangedAttacker).At(2, 0)
            //    .AddEntity().IsPlayer(true).WithHealth(5).WithEnergy(5).WithActions(ActionPresets.MeleeAttacker).At(2, 4)
            //    .Build();

            //BestPlan best = scenario.RunAI(depth);
            //throw new System.NotImplementedException("FIX THIS");
            //List<ExecutedTask> executedTasks = best.ExecutedTasks.ToList();
            //Assert.That(executedTasks.Count, Is.EqualTo(2));
            //Assert.That(executedTasks[0].Action, Is.TypeOf<AttackAction>());
            //Assert.That(executedTasks[1].Action, Is.TypeOf<AttackAction>());
        }
    }
}