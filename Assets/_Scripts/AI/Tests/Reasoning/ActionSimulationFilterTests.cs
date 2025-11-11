using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Action = MagmaHeart.AI.Actions.Action;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class ActionSimulationFilterTests
    {
        private Board m_board;

        private Func<int, Vector2, bool, Entity> Entity = (health, position, isPlayer) =>
        {
            Entity entity = new Entity(health, position, isPlayer);
            entity.PossibleActions.Add(new AttackAction(entity, 4));
            entity.PossibleActions.Add(new MoveAction(entity, 3));
            entity.PossibleActions.Add(new EngageAction(entity, 4, 1));
            entity.PossibleActions.Add(new RunAwayAction(entity, 3));
            return entity;
        };

        [SetUp]
        public void SetUp()
        {
            BoardGraph graph = new BoardGraph();
            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 5; ++j)
                    graph.AddNode(new Vector2(i, j), BoardNodeType.Walkable);

            m_board = new Board(graph);
        }

        [Test]
        public void GetActionSimulations_From2PossibleActions_Returns2ActionsWith1PossibleArgument()
        {
            Entity player = Entity(10, new Vector2(5, 5), true);
            Entity enemy = Entity(10, Vector2.zero, false);
            List<Action> possibleActions = enemy.PossibleActions.ToList();
            StateSnapshot stateSnapshot = StateSnapshotMaker.CreateStateSnapshot(new CircularList<AIUnit>() { enemy, player });
            SimulatedBoard simulatedBoard = m_board.CreateSimulatedBoard();

            List<ActionSimulation> actionSimulations = ActionSimulationFilter.GetActionSimulations(stateSnapshot, simulatedBoard, possibleActions);
            
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
            Entity player = Entity(10, new Vector2(5, 5), true);
            Entity enemy = Entity(10, new Vector2(5, 3), false);
            List<Action> possibleActions = enemy.PossibleActions.ToList();
            StateSnapshot stateSnapshot = StateSnapshotMaker.CreateStateSnapshot(new CircularList<AIUnit>() { enemy, player });
            SimulatedBoard simulatedBoard = m_board.CreateSimulatedBoard();

            List<ActionSimulation> actionSimulations = ActionSimulationFilter.GetActionSimulations(stateSnapshot, simulatedBoard, possibleActions);
            
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
            Entity player1 = Entity(10, new Vector2(2, 3), true);
            Entity player2 = Entity(10, new Vector2(4, 3), true);
            Entity enemy = Entity(10, new Vector2(3, 3), false);
            List<Action> possibleActions = enemy.PossibleActions.ToList();
            StateSnapshot stateSnapshot = StateSnapshotMaker.CreateStateSnapshot(new CircularList<AIUnit>() { enemy, player1, player2 });
            SimulatedBoard simulatedBoard = m_board.CreateSimulatedBoard();

            List<ActionSimulation> actionSimulations = ActionSimulationFilter.GetActionSimulations(stateSnapshot, simulatedBoard, possibleActions);

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

        private List<ActionArgs> GetArguments<T>(List<ActionSimulation> simulations) where T : Action
        {
            return simulations.Select(s => s)
                .Where(s => s.Action.GetType() == typeof(T))
                .First()
                .SimulationArgs;
        }
    }
}
