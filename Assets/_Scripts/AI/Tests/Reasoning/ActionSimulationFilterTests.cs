using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class ActionSimulationFilterTests : ReasoningTests
    {
        [Test]
        public void GetActionSimulations_From2PossibleActions_Returns2ActionsWith1PossibleArgument()
        {
            Entity player = CreateEntity(10, new Vector2(5, 5), true);
            Entity enemy = CreateEntity(10, Vector2.zero, false);
            SimulatedBoardState simulation = new SimulatedBoardState(Board);
            ActionSimulationFilter filter = new ActionSimulationFilter(Strategy, Database);

            List<PlanSimulation> actionSimulations = filter.GetPossiblePlans(simulation, enemy);
            
            Assert.That(actionSimulations.Count, Is.EqualTo(4));
            Assert.That(actionSimulations.Any(a => a.Plan.Tasks.First().Action.GetType() == typeof(MoveAction)));
            Assert.That(actionSimulations.Any(a => a.Plan.Tasks.First().Action.GetType() == typeof(RunAwayAction)));
            Assert.That(actionSimulations.Any(a => a.Plan.Tasks.First().Action.GetType() == typeof(AttackAction)));
            Assert.That(actionSimulations.Any(a => a.Plan.Tasks.First().Action.GetType() == typeof(EngageAction)));
            Assert.That(actionSimulations.All(simulation => simulation.Targets.Count() == 1));
            Assert.That(actionSimulations.All(simulation => simulation.Targets.First() == player));
        }
    }
}
