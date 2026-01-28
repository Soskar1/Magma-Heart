using MagmaHeart.AI.Boards;
using MagmaHeart.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.States
{
    public sealed class SimulatedBoardState : BoardState
    {
        private readonly Dictionary<int, TypeMap<PropertySnapshot>> m_stateProperties;
        private readonly Stack<SimulationChange> m_history;

        internal Stack<SimulationChange> History => m_history;

        public SimulatedBoardState(Board board) : base(board.DeepCopy())
        {
            m_stateProperties = new Dictionary<int, TypeMap<PropertySnapshot>>();

            foreach (AIUnitModel unit in Board.GetUnits())
            {
                TypeMap<PropertySnapshot> unitProperties = unit.GetPropertySnapshots();
                m_stateProperties[unit.Id] = unitProperties;
            }

            m_history = new Stack<SimulationChange>();
        }

        internal override void ApplyStateChanges(IEnumerable<StateChange> stateChanges)
        {
            base.ApplyStateChanges(stateChanges);

            SimulationChange change = new SimulationChange(new List<StateChange>(stateChanges.ToList()));
            m_history.Push(change);
        }

        public void Undo()
        {
            List<StateChange> stateChanges = m_history.Pop().StateChanges;

            for (int i = stateChanges.Count - 1; i >= 0; --i)
            {
                StateChange stateChange = stateChanges[i];
                stateChange.UndoChangeToSimulation(this);
            }
        }

        public override T GetProperty<T>(AIUnitModel unit) => (T)GetProperty(unit, typeof(T));
        private PropertySnapshot GetProperty(AIUnitModel unit, Type propertyType) => m_stateProperties[unit.Id][propertyType];

        public void ProduceStateChange(StateChange stateChange) => stateChange.ApplyChangeToSimulation(this);
        public void UndoStateChange(StateChange stateChange) => stateChange.UndoChangeToSimulation(this);
    }
}