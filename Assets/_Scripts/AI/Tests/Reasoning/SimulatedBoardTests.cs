using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.AI.States.SimulationOperations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal class SimulatedBoardTests
    {
        private Board m_board;
        private Entity m_entity;
        private SimulatedBoardState m_state;

        internal class EmptyAction : UnitAction
        {
            public override bool CanExecute(ActionArgs args, BoardState boardState) => true;

            public override IEnumerable<StateChange> ProduceChanges(ActionArgs args, BoardState boardState) => new List<StateChange>();

            public override bool TryCreateArgs(ActionInput input, ActionData data, BoardState baordState, out ActionArgs args)
            {
                args = null;
                return true;
            }

            public override bool TryGenerateArgs(AIUnitModel executor, ActionData data, BoardState boardState, out ActionArgs args)
            {
                args = null;
                return false;
            }
        }

        [SetUp]
        public void SetUp()
        {
            m_entity = new Entity(10, Vector2.zero, false);

            BoardGraph graph = new BoardGraph();
            graph.AddNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(Vector2.up, BoardNodeType.Walkable);

            m_board = new Board(graph);
            m_board.AddUnit(m_entity.Position, m_entity);

            m_state = new SimulatedBoardState(m_board);
        }

        [Test]
        public void SimulatedBoardState_CreatesSimulatedBoardWithUnitProperties()
        {
            Assert.That(ReferenceEquals(m_board, m_state.Board), Is.EqualTo(false));
            Assert.That(m_state.GetProperty<Health>(m_entity).CurrentHealth, Is.EqualTo(m_entity.Health));
            Assert.That(m_state.GetProperty<Position>(m_entity).CurrentPosition, Is.EqualTo(m_entity.Position));
        }

        [Test]
        public void GetWriteProperty_UpdatesPropertiesInSimulation()
        {
            m_state.UpdateProperty(m_entity, new Position(Vector2.up));
            m_state.UpdateProperty(m_entity, new Health(4, 10));

            Assert.That(m_state.GetProperty<Health>(m_entity).CurrentHealth, Is.EqualTo(4));
            Assert.That(m_state.GetProperty<Position>(m_entity).CurrentPosition, Is.EqualTo(Vector2.up));
            Assert.That(m_entity.Position, Is.EqualTo(Vector2.zero));
            Assert.That(m_entity.Health, Is.EqualTo(10));
        }

        [Test]
        public void ApplyStateChanges_AddsStateChangeToHistoryStack()
        {
            MoveAction moveAction = new MoveAction();
            TargetPositionActionInput input = new TargetPositionActionInput(m_entity, Vector2.up);
            MoveActionArgs args = new MoveActionArgs(input, new MoveActionData(1));

            moveAction.Execute(args, m_state);

            Assert.That(m_state.History.Count, Is.EqualTo(1));
            List<SimulationOperation> operations = m_state.History.Peek().Operations;
            Assert.That(operations.Count, Is.EqualTo(3));
            Assert.That(operations[2] is UnitPropertyUpdateSimulationOperation, Is.EqualTo(true));
            Assert.That(operations[1] is AddUnitBoardSimulationOperation, Is.EqualTo(true));
            Assert.That(operations[0] is RemoveUnitBoardSimulationOperation, Is.EqualTo(true));
        }

        [Test]
        public void Undo_RemovesLastStateChangeFromHistoryStack()
        {
            MovementStateChange movementStateChange = new MovementStateChange(m_entity, Vector2.zero, Vector2.up);
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange });

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(0));
        }

        [Test]
        public void ApplyStateChanges_ActionAppliesTwoStateChanges_AddsTwoStateChangesToHistoryStack()
        {
            Entity player = new Entity(10, Vector2.up, true);
            m_board.AddUnit(player.Position, player);
            m_state = new SimulatedBoardState(m_board);
            EngageAction action = new EngageAction();
            TargetEntityActionInput input = new TargetEntityActionInput(m_entity, player);
            EngageActionArgs args = new EngageActionArgs(input, new EngageActionData(1, 1));

            action.Execute(args, m_state);

            Assert.That(m_state.History.Count, Is.EqualTo(1));
            List<SimulationOperation> operations = m_state.History.Peek().Operations;
            Assert.That(operations.Count, Is.EqualTo(4));
            Assert.That(m_state.GetProperty<Position>(m_entity).CurrentPosition, Is.EqualTo(player.Position));
            Assert.That(m_state.GetProperty<Health>(player).CurrentHealth, Is.EqualTo(9));
            Assert.That(m_entity.Position, Is.EqualTo(Vector2.zero));
            Assert.That(player.Health, Is.EqualTo(10));
        }

        [Test]
        public void Undo_ActionAppliesTwoStateChanges_RemovesTwoStateChangesFromSimulation()
        {
            Entity player = new Entity(10, Vector2.up, true);
            m_board.AddUnit(player.Position, player);
            m_state = new SimulatedBoardState(m_board);
            EngageAction action = new EngageAction();
            TargetEntityActionInput input = new TargetEntityActionInput(m_entity, player);
            EngageActionArgs args = new EngageActionArgs(input, new EngageActionData(1, 1));
            action.Execute(args, m_state);

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(0));
            Assert.That(m_state.GetProperty<Position>(m_entity).CurrentPosition, Is.EqualTo(Vector2.zero));
            Assert.That(m_state.GetProperty<Health>(player).CurrentHealth, Is.EqualTo(10));
        }

        [Test]
        public void Undo_AfterTwoStateChangesAndEmptyAction_DoesNotDeleteFirstActionStateChanges()
        {
            Entity player = new Entity(10, Vector2.up, true);
            m_board.AddUnit(player.Position, player);
            m_state = new SimulatedBoardState(m_board);
            EngageAction action = new EngageAction();
            TargetEntityActionInput input = new TargetEntityActionInput(m_entity, player);
            EngageActionArgs args = new EngageActionArgs(input, new EngageActionData(1, 1));
            EmptyAction emptyAction = new EmptyAction();
            action.Execute(args, m_state);
            emptyAction.Execute(new ActionArgs(input), m_state);

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(1));
            List<SimulationOperation> operations = m_state.History.Peek().Operations;
            Assert.That(operations.Count, Is.EqualTo(4));
        }
    }
}
