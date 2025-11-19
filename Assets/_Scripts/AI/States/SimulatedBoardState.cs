using MagmaHeart.AI.Boards;
using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.AI.States
{
    public sealed class SimulatedBoardState : BoardState
    {
        private Dictionary<AIUnit, TypeMap<PropertySnapshot>> m_stateProperties;
        private Stack<SimulationChange> m_history;

        internal Stack<SimulationChange> History => m_history;

        public SimulatedBoardState(Board board) : base(board.DeepCopy())
        {
            m_stateProperties = new Dictionary<AIUnit, TypeMap<PropertySnapshot>>();

            foreach (AIUnit unit in Board.GetUnits())
            {
                TypeMap<PropertySnapshot> unitProperties = unit.GetPropertySnapshots();
                m_stateProperties[unit] = unitProperties;
            }

            m_history = new Stack<SimulationChange>();
        }

        public override void ApplyStateChanges(List<StateChange> stateChanges)
        {
            base.ApplyStateChanges(stateChanges);

            SimulationChange change = new SimulationChange(stateChanges);
            m_history.Push(change);
        }

        public void Undo()
        {
            SimulationChange change = m_history.Pop();
            foreach (StateChange state in change.StateChanges)
                state.UndoChangeInSimulation(this);
        }

        public override T GetProperty<T>(AIUnit unit) => (T)m_stateProperties[unit][typeof(T)];
        public void WriteProperty<T>(AIUnit unit, T property) where T : PropertySnapshot => m_stateProperties[unit][typeof(T)] = property;
    }
}