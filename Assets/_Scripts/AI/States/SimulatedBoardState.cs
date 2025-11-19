using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States.SimulationOperations;
using MagmaHeart.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.States
{
    public sealed class SimulatedBoardState : BoardState
    {
        private readonly Dictionary<AIUnit, TypeMap<PropertySnapshot>> m_stateProperties;
        private readonly Stack<SimulationChange> m_history;
        private readonly List<SimulationOperation> m_currentSimulationOperations;

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
            m_currentSimulationOperations = new List<SimulationOperation>();
        }

        public override void ApplyStateChanges(List<StateChange> stateChanges)
        {
            base.ApplyStateChanges(stateChanges);

            SimulationChange change = new SimulationChange(new List<SimulationOperation>(m_currentSimulationOperations));
            m_history.Push(change);

            m_currentSimulationOperations.Clear();
        }

        public void Undo()
        {
            List<SimulationOperation> operations = m_history.Pop().Operations;
            for (int i = operations.Count - 1; i >= 0; --i)
            {
                SimulationOperation operation = operations[i];

                if (operation is AddUnitBoardSimulationOperation addUnitOperation)
                    Board.RemoveUnit(addUnitOperation.Position, addUnitOperation.AddedUnit);
                else if (operation is RemoveUnitBoardSimulationOperation removeUnitOperation)
                    Board.AddUnit(removeUnitOperation.Position, removeUnitOperation.RemovedUnit);
                else if (operation is UnitPropertyUpdateSimulationOperation unitPropertyUpdateOperation)
                    WriteProperty(unitPropertyUpdateOperation.Unit, unitPropertyUpdateOperation.OldPropertyValue);

                // TODO: Implement ChangeNodeType
            }
        }

        public override T GetProperty<T>(AIUnit unit) => (T)GetProperty(unit, typeof(T));
        private PropertySnapshot GetProperty(AIUnit unit, Type propertyType) => m_stateProperties[unit][propertyType];
        private void WriteProperty(AIUnit unit, PropertySnapshot property)
        {
            Type propertyType = property.GetType();
            m_stateProperties[unit][propertyType] = property;
        }

        public void UpdateProperty(AIUnit unit, PropertySnapshot property)
        {
            Type propertyType = property.GetType();
            PropertySnapshot oldValue = GetProperty(unit, propertyType);

            WriteProperty(unit, property);

            UnitPropertyUpdateSimulationOperation operation = new UnitPropertyUpdateSimulationOperation(unit, oldValue, property);
            m_currentSimulationOperations.Add(operation);
        }

        public void AddUnit(Vector2 position, AIUnit unit)
        {
            Board.AddUnit(position, unit);

            AddUnitBoardSimulationOperation operation = new AddUnitBoardSimulationOperation(position, unit);
            m_currentSimulationOperations.Add(operation);
        }

        public void RemoveUnit(Vector2 position, AIUnit unit)
        {
            Board.RemoveUnit(position, unit);

            RemoveUnitBoardSimulationOperation operation = new RemoveUnitBoardSimulationOperation(position, unit);
            m_currentSimulationOperations.Add(operation);
        }
    }
}