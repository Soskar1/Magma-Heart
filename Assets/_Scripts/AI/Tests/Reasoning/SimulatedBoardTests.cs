using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
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
            m_entity = new Entity(false, 0);
            m_entity.CurrentHealth = 10;
            m_entity.Position = Vector2.zero;

            BoardGraph graph = new BoardGraph();
            graph.AddNode(Vector2.zero, BoardNodeType.Walkable);
            graph.AddNode(Vector2.up, BoardNodeType.Walkable);
            graph.AddNode(new Vector2(1, 1), BoardNodeType.Walkable);

            m_board = new Board(graph);
            m_board.AddUnit(m_entity.Position, m_entity);

            m_state = new SimulatedBoardState(m_board);
        }

        [Test]
        public void SimulatedBoardState_CreatesSimulatedBoardWithUnitProperties()
        {
            m_state.Board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);

            Assert.That(ReferenceEquals(m_board, m_state.Board), Is.EqualTo(false));
            Assert.That(simulatedEntity.CurrentHealth, Is.EqualTo(m_entity.CurrentHealth));
            Assert.That(simulatedEntity.Position, Is.EqualTo(m_entity.Position));
        }

        [Test]
        public void Undo_RemovesLastStateChangeFromHistoryStack()
        {
            MovementStateChange movementStateChange = new MovementStateChange(m_entity.Id, Vector2.zero, Vector2.up);
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange });

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(0));
        }

        [Test]
        public void Undo_TwoSimulations_RemovesLastStateChangeFromHistoryStack()
        {
            MovementStateChange movementStateChange = new MovementStateChange(m_entity.Id, Vector2.zero, Vector2.up);
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange });
            movementStateChange = new MovementStateChange(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange });

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(1));
            m_state.Board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(Vector2.up));
        }

        [Test]
        public void Undo_MultipleStateChangesInOneSimulation_RemovesLastStateChangeFromHistoryStack()
        {
            MovementStateChange movementStateChange1 = new MovementStateChange(m_entity.Id, Vector2.zero, Vector2.up);
            MovementStateChange movementStateChange2 = new MovementStateChange(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange1, movementStateChange2 });

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(0));
            m_state.Board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(Vector2.zero));
        }

        [Test]
        public void Undo_TwoSimulationsWithMultipleStateChangesInOneSimulation_RemovesLastStateChangeFromHistoryStack()
        {
            MovementStateChange movementStateChange1 = new MovementStateChange(m_entity.Id, Vector2.zero, Vector2.up);
            MovementStateChange movementStateChange2 = new MovementStateChange(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange1, movementStateChange2 });
            movementStateChange1 = new MovementStateChange(m_entity.Id, new Vector2(1, 1), Vector2.up);
            movementStateChange2 = new MovementStateChange(m_entity.Id, Vector2.up, Vector2.zero);
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange1, movementStateChange2 });

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(1));
            m_state.Board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(new Vector2(1, 1)));
        }

        [Test]
        public void Undo_TwoSimulationsWithMultipleStateChangesInOneSimulationMultipleUndos_ReturnToInitialState()
        {
            MovementStateChange movementStateChange1 = new MovementStateChange(m_entity.Id, Vector2.zero, Vector2.up);
            MovementStateChange movementStateChange2 = new MovementStateChange(m_entity.Id, Vector2.up, new Vector2(1, 1));
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange1, movementStateChange2 });
            movementStateChange1 = new MovementStateChange(m_entity.Id, new Vector2(1, 1), Vector2.up);
            movementStateChange2 = new MovementStateChange(m_entity.Id, Vector2.up, Vector2.zero);
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange1, movementStateChange2 });

            m_state.Undo();
            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(0));
            m_state.Board.TryGetUnit(m_entity.Id, out Entity simulatedEntity);
            Assert.That(simulatedEntity.Position, Is.EqualTo(Vector2.zero));
        }
    }
}
