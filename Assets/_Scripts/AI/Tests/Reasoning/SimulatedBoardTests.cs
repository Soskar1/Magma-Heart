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

            m_board = new Board(graph);
            m_board.AddUnit(m_entity.Position, m_entity);

            m_state = new SimulatedBoardState(m_board);
        }

        [Test]
        public void SimulatedBoardState_CreatesSimulatedBoardWithUnitProperties()
        {
            Assert.That(ReferenceEquals(m_board, m_state.Board), Is.EqualTo(false));
            Assert.That(m_state.GetProperty<Health>(m_entity).CurrentHealth, Is.EqualTo(m_entity.CurrentHealth));
            Assert.That(m_state.GetProperty<Position>(m_entity).CurrentPosition, Is.EqualTo(m_entity.Position));
        }

        [Test]
        public void Undo_RemovesLastStateChangeFromHistoryStack()
        {
            MovementStateChange movementStateChange = new MovementStateChange(m_entity.Id, Vector2.zero, Vector2.up);
            m_state.ApplyStateChanges(new List<StateChange>() { movementStateChange });

            m_state.Undo();

            Assert.That(m_state.History.Count, Is.EqualTo(0));
        }
    }
}
