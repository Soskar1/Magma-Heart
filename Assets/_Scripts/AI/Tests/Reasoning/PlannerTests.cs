using MagmaHeart.AI.Reasoning.Plans;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class PlannerTests : ReasoningTests
    {
        [Test]
        public void GetActionSimulations_From2PossibleActions_Returns2ActionsWith1PossibleArgument()
        {
            Entity player = CreateEntity(10, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(10, Vector2.zero, false);
            Planner planner = new Planner(Strategy, Database, new Execution.CommandRunner());

            List<Plan> plans = planner.GetPlans(enemy);
            
            Assert.That(plans.Count, Is.EqualTo(3));
            Assert.That(plans.Any(a => a.Tasks.First().Action.GetType() == typeof(MoveAction)));
            Assert.That(plans.Any(a => a.Tasks.First().Action.GetType() == typeof(RunAwayAction)));
            Assert.That(plans.Any(a => a.Tasks.First().Action.GetType() == typeof(AttackAction)));
        }
    }
}
