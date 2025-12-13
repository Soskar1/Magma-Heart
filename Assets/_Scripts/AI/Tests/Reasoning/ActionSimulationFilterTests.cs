using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnitAction = MagmaHeart.AI.Actions.UnitAction;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class ActionSimulationFilterTests
    {
        private Board m_board;
        private DefaultArgumentResolver m_argumentResolver;

        private Func<int, Vector2, bool, Board, Entity> Entity = (health, position, isPlayer, board) =>
        {
            Entity entity = new Entity(health, position, isPlayer);
            entity.PossibleActions.Add(new AttackAction(entity, 4));
            entity.PossibleActions.Add(new MoveAction(entity, 3));
            entity.PossibleActions.Add(new EngageAction(entity, 4, 1));
            entity.PossibleActions.Add(new RunAwayAction(entity, 3));
            board.AddUnit(position, entity);

            return entity;
        };

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = 0; i < 6; ++i)
                for (int j = 0; j < 6; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            m_board = new Board(graph);
            m_argumentResolver = new DefaultArgumentResolver();
        }

        [Test]
        public void GetActionSimulations_From2PossibleActions_Returns2ActionsWith1PossibleArgument()
        {
            Entity player = Entity(10, new Vector2(5, 5), true, m_board);
            Entity enemy = Entity(10, Vector2.zero, false, m_board);
            SimulatedBoardState simulation = new SimulatedBoardState(m_board);

            List<ActionSimulation> actionSimulations = ActionSimulationFilter.GetActionSimulations(simulation, enemy, m_argumentResolver);
            
            Assert.That(actionSimulations.Count, Is.EqualTo(2));
            Assert.That(actionSimulations.Any(a => a.Action is MoveAction));
            Assert.That(actionSimulations.Any(a => a.Action is RunAwayAction));
            List<MoveActionArgs> moveActionArgs = GetArguments<MoveAction>(actionSimulations).Select(a => (MoveActionArgs)a).ToList();
            Assert.That(moveActionArgs.Count, Is.EqualTo(1));
            Assert.That(moveActionArgs.First().Target, Is.EqualTo(player.Position));
            List<RunAwayActionArgs> runAwayActionArgs = GetArguments<RunAwayAction>(actionSimulations).Select(a => (RunAwayActionArgs)a).ToList();
            Assert.That(runAwayActionArgs.Count, Is.EqualTo(1));
            Assert.That(runAwayActionArgs.First().RunAwayFrom, Is.EqualTo(player));
        }

        [Test]
        public void GetActionSimulations_From3PossibleActions_Returns3ActionsWith1PossibleArgument()
        {
            Entity player = Entity(10, new Vector2(5, 5), true, m_board);
            Entity enemy = Entity(10, new Vector2(5, 3), false, m_board);
            SimulatedBoardState simulation = new SimulatedBoardState(m_board);

            List<ActionSimulation> actionSimulations = ActionSimulationFilter.GetActionSimulations(simulation, enemy, m_argumentResolver);
            
            Assert.That(actionSimulations.Count, Is.EqualTo(3));
            Assert.That(actionSimulations.Any(a => a.Action is MoveAction));
            Assert.That(actionSimulations.Any(a => a.Action is EngageAction));
            Assert.That(actionSimulations.Any(a => a.Action is RunAwayAction));
            List<MoveActionArgs> moveActionArgs = GetArguments<MoveAction>(actionSimulations).Select(a => (MoveActionArgs)a).ToList();
            Assert.That(moveActionArgs.Count, Is.EqualTo(1));
            Assert.That(moveActionArgs.First().Target, Is.EqualTo(player.Position));
            List<EngageActionArgs> engageActionArgs = GetArguments<EngageAction>(actionSimulations).Select(a => (EngageActionArgs)a).ToList();
            Assert.That(engageActionArgs.Count, Is.EqualTo(1));
            Assert.That(engageActionArgs.First().Target, Is.EqualTo(player));
            List<RunAwayActionArgs> runAwayActionArgs = GetArguments<RunAwayAction>(actionSimulations).Select(a => (RunAwayActionArgs)a).ToList();
            Assert.That(runAwayActionArgs.Count, Is.EqualTo(1));
            Assert.That(runAwayActionArgs.First().RunAwayFrom, Is.EqualTo(player));
        }

        [Test]
        public void GetActionSimulations_OneEnemyBetweenTwoPlayers_Returns2ActionsWith2PossibleArguments()
        {
            Entity player1 = Entity(10, new Vector2(2, 3), true, m_board);
            Entity player2 = Entity(10, new Vector2(4, 3), true, m_board);
            Entity enemy = Entity(10, new Vector2(3, 3), false, m_board);
            SimulatedBoardState simulation = new SimulatedBoardState(m_board);

            List<ActionSimulation> actionSimulations = ActionSimulationFilter.GetActionSimulations(simulation, enemy, m_argumentResolver);

            Assert.That(actionSimulations.Count, Is.EqualTo(2));
            Assert.That(actionSimulations.Any(a => a.Action is AttackAction));
            Assert.That(actionSimulations.Any(a => a.Action is RunAwayAction));
            List<AttackActionArgs> attackActionArgs = GetArguments<AttackAction>(actionSimulations).Select(a => (AttackActionArgs)a).ToList();
            Assert.That(attackActionArgs.Count, Is.EqualTo(2));
            Assert.That(attackActionArgs.Any(a => a.Target == player1));
            Assert.That(attackActionArgs.Any(a => a.Target == player2));
            List<RunAwayActionArgs> runAwayActionArgs = GetArguments<RunAwayAction>(actionSimulations).Select(a => (RunAwayActionArgs)a).ToList();
            Assert.That(runAwayActionArgs.Count, Is.EqualTo(2));
            Assert.That(runAwayActionArgs.Any(a => a.RunAwayFrom == player1));
            Assert.That(runAwayActionArgs.Any(a => a.RunAwayFrom == player2));
        }

        private List<ActionArgs> GetArguments<T>(List<ActionSimulation> simulations) where T : UnitAction
        {
            return simulations.Select(s => s)
                .Where(s => s.Action.GetType() == typeof(T))
                .First()
                .SimulationArgs;
        }
    }
}
