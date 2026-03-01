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

            IList<AbilityPlan> best = scenario.RunAI(depth, TestDatabase);

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
            AIScenario scenario = AIScenarioBuilder.Create(World)
                .AddEntity().IsPlayer(false).WithData(EntityDatabase.SkeletonWarrior).At(3, 3)
                .AddEntity().IsPlayer(true).WithData(EntityDatabase.Warrior).At(2, 3)
                .Build();

            IList<AbilityPlan> best = scenario.RunAI(depth, TestDatabase);

            Assert.That(best.Count, Is.EqualTo(1));
            Assert.That(best.First().Effects, Has.Some.InstanceOf<DamageEffect>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void AttackPlan_EnemyWithLowHealthAndAggressiveStrategy_AttacksPlayer(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(World)
                .AddEntity().IsPlayer(false).WithParameterValue(TestDatabase.Health, 1).WithData(EntityDatabase.SkeletonWarrior).At(3, 3)
                .AddEntity().IsPlayer(true).WithData(EntityDatabase.Warrior).At(2, 3)
                .Build();

            IList<AbilityPlan> best = scenario.RunAI(depth, TestDatabase);

            Assert.That(best.Count(), Is.EqualTo(1));
            Assert.That(best.First().Effects, Has.Some.InstanceOf<DamageEffect>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Null_PlayerSurroundedByWalls_EnemyDoesNothing(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(World)
                .AddEntity().IsPlayer(false).WithData(EntityDatabase.SkeletonWarrior).At(4, 3)
                .AddEntity().IsPlayer(true).WithData(EntityDatabase.Warrior).At(2, 3)
                .ModifyBoard().SurroundWithWalls(4, 3).Bake()
                .Build();

            IList<AbilityPlan> best = scenario.RunAI(depth, TestDatabase);

            Assert.That(best, Is.Null);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void AttackPlan_OnePlayerTwoEnemiesWithLowHealth_EnemyMovesTowardsPlayerAndAttacksHim(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(World)
                .AddEntity()
                    .IsPlayer(false)
                    .WithParameterValue(TestDatabase.Health, 1)
                    .WithParameterValue(TestDatabase.Energy, 5)
                    .WithData(EntityDatabase.SkeletonWarrior)
                    .At(1, 1)
                .AddEntity()
                    .IsPlayer(true)
                    .WithParameterValue(TestDatabase.Health, 4)
                    .WithData(EntityDatabase.Warrior)
                    .At(3, 3)
                .AddEntity()
                    .IsPlayer(false)
                    .WithParameterValue(TestDatabase.Health, 2)
                    .WithData(EntityDatabase.SkeletonWarrior)
                    .At(3, 2)
                .Build();

            IList<AbilityPlan> best = scenario.RunAI(depth, TestDatabase);

            Assert.That(best.Count, Is.EqualTo(1));
            Assert.That(best.First().Effects, Has.Some.InstanceOf<MoveEffect>());
            Assert.That(best.First().Effects, Has.Some.InstanceOf<DamageEffect>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void AttackPlan_OnePlayerTwoEnemies_EnemyMovesTowardsPlayerAndAttacksHim(int depth)
        {
            AIScenario scenario = AIScenarioBuilder.Create(World)
                .AddEntity()
                    .IsPlayer(false)
                    .WithParameterValue(TestDatabase.Health, 2)
                    .WithParameterValue(TestDatabase.Energy, 5)
                    .WithData(EntityDatabase.SkeletonWarrior)
                    .At(0, 1)
                .AddEntity()
                    .IsPlayer(true)
                    .WithParameterValue(TestDatabase.Health, 5)
                    .WithParameterValue(TestDatabase.Energy, 5)
                    .WithData(EntityDatabase.Warrior)
                    .At(2, 2)
                .AddEntity()
                    .IsPlayer(false)
                    .WithParameterValue(TestDatabase.Health, 5)
                    .WithParameterValue(TestDatabase.Energy, 5)
                    .WithData(EntityDatabase.SkeletonWarrior)
                    .At(2, 1)
                .Build();

            IList<AbilityPlan> executedAbilities = scenario.RunAI(depth, TestDatabase);

            Assert.That(executedAbilities.Count, Is.EqualTo(2));
            Assert.That(executedAbilities[0].Effects, Has.Some.InstanceOf<MoveEffect>());
            Assert.That(executedAbilities[0].Effects, Has.Some.InstanceOf<DamageEffect>());
            Assert.That(executedAbilities[1].Effects, Has.Some.InstanceOf<DamageEffect>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void RangedAttack_WallPlacedBetweenEnemyAndPlayer_EnemyDoNotUseRangedAttackAsAFirstAction(int depth)
        {
            //AIScenario scenario = AIScenarioBuilder.Create(World)
            //    .AddEntity()
            //        .IsPlayer(false)
            //        .WithHealth(2)
            //        .WithEnergy(5)
            //        .WithActions(ActionPresets.RangedAttacker)
            //        .At(2, 0)
            //    .AddEntity().IsPlayer(true).WithHealth(5).WithEnergy(5).WithActions(ActionPresets.MeleeAttacker).At(2, 2)
            //    .ModifyBoard().PlaceWallAt(2, 1).Bake()
            //    .Build();
            throw new System.NotImplementedException("FIX THIS");
            //BestPlan best = scenario.RunAI(depth);
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
            throw new System.NotImplementedException("FIX THIS");

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