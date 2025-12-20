using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnitAction = MagmaHeart.AI.Actions.UnitAction;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class ActionSimulationFilterTests
    {
        private Board m_board;
        private BasicStrategy m_strategy;
        private ActionDatabase m_database;

        private Func<int, Vector2, bool, Board, Entity> Entity = (health, position, isPlayer, board) =>
        {
            Entity entity = new Entity(health, position, isPlayer);
            AttackActionData attackData = new AttackActionData(4);
            MoveActionData moveData = new MoveActionData(3);
            EngageActionData engageData = new EngageActionData(4, 1);
            RunAwayActionData runAwayData = new RunAwayActionData(3);

            entity.PossibleActions.Add(attackData.GetDefinition());
            entity.PossibleActions.Add(moveData.GetDefinition());
            entity.PossibleActions.Add(engageData.GetDefinition());
            entity.PossibleActions.Add(runAwayData.GetDefinition());
            board.AddUnit(position, entity);

            return entity;
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            m_database = new ActionDatabase(assembly);
            m_strategy = new BasicStrategy(null);
        }   

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = 0; i < 6; ++i)
                for (int j = 0; j < 6; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            m_board = new Board(graph);
        }

        [Test]
        public void GetActionSimulations_From2PossibleActions_Returns2ActionsWith1PossibleArgument()
        {
            Entity player = Entity(10, new Vector2(5, 5), true, m_board);
            Entity enemy = Entity(10, Vector2.zero, false, m_board);
            SimulatedBoardState simulation = new SimulatedBoardState(m_board);
            ActionSimulationFilter filter = new ActionSimulationFilter(m_strategy, m_database);

            List<PlanSimulation> actionSimulations = filter.GetPossiblePlans(simulation, enemy);
            
            Assert.That(actionSimulations.Count, Is.EqualTo(4));
            Assert.That(actionSimulations.Any(a => a.Plan.Task.Action.GetType() == typeof(MoveAction)));
            Assert.That(actionSimulations.Any(a => a.Plan.Task.Action.GetType() == typeof(RunAwayAction)));
            Assert.That(actionSimulations.Any(a => a.Plan.Task.Action.GetType() == typeof(AttackAction)));
            Assert.That(actionSimulations.Any(a => a.Plan.Task.Action.GetType() == typeof(EngageAction)));
            List<MoveActionArgs> moveActionArgs = GetArguments<MoveAction>(actionSimulations).Select(a => (MoveActionArgs)a).ToList();
            Assert.That(moveActionArgs.Count, Is.EqualTo(1));
            Assert.That(moveActionArgs.First().Target, Is.EqualTo(player.Position));
            List<RunAwayActionArgs> runAwayActionArgs = GetArguments<RunAwayAction>(actionSimulations).Select(a => (RunAwayActionArgs)a).ToList();
            Assert.That(runAwayActionArgs.Count, Is.EqualTo(1));
            Assert.That(runAwayActionArgs.First().RunAwayFrom, Is.EqualTo(player));
            List<AttackActionArgs> attackActionArgs = GetArguments<AttackAction>(actionSimulations).Select(a => (AttackActionArgs)a).ToList();
            Assert.That(attackActionArgs.Count, Is.EqualTo(1));
            Assert.That(attackActionArgs.First().Damage, Is.EqualTo(4));
            Assert.That(attackActionArgs.First().Target, Is.EqualTo(player));
            List<EngageActionArgs> engageActionArgs = GetArguments<EngageAction>(actionSimulations).Select(a => (EngageActionArgs)a).ToList();
            Assert.That(engageActionArgs.Count, Is.EqualTo(1));
            Assert.That(engageActionArgs.First().Damage, Is.EqualTo(4));
            Assert.That(engageActionArgs.First().Target, Is.EqualTo(player));
        }

        private List<ActionArgs> GetArguments<T>(List<PlanSimulation> simulations) where T : UnitAction
        {
            return simulations.Select(s => s)
                .Where(s => s.Plan.Task.Action.GetType() == typeof(T))
                .First()
                .SimulationArgs;
        }
    }
}
