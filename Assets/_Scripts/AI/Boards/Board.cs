using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class Board
    {
        public BoardGraph Graph { get; init; }

        private IDictionary<Vector2, AIUnitModel> m_units;
        private IDictionary<int, AIUnitModel> m_unitsById;
        private IDictionary<int, Vector2> m_unitPositions;

        public Board(BoardGraph graph)
        {
            Graph = graph;
            m_units = new Dictionary<Vector2, AIUnitModel>();
            m_unitsById = new Dictionary<int, AIUnitModel>();
            m_unitPositions = new Dictionary<int, Vector2>();
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => Graph.ChangeNodeType(position, newNodeType);
        public BoardNodeType GetNodeType(Vector2 position) => Graph.GetNode(position).Type;

        public void AddUnit(Vector2 position, AIUnitModel unit)
        {
            if (Graph.GetNode(position) == null)
                throw new ArgumentException($"Node at {position} does not exist in the board graph!");

            m_units[position] = unit;
            m_unitsById[unit.Id] = unit;
            m_unitPositions[unit.Id] = position;
        }

        public bool RemoveUnit(Vector2 position)
        {
            if (!m_units.ContainsKey(position))
                return false;

            AIUnitModel model = m_units[position];
            m_unitsById.Remove(model.Id);
            m_unitPositions.Remove(model.Id);

            return m_units.Remove(position);
        }

        public bool TryGetUnit(Vector2 position, out AIUnitModel unit) => m_units.TryGetValue(position, out unit);
        public bool TryGetUnit<T>(Vector2 position, out T unit) where T : AIUnitModel
        {
            if (TryGetUnit(position, out AIUnitModel aiUnitModel))
            {
                if (aiUnitModel is T typedUnit)
                {
                    unit = typedUnit;
                    return true;
                }
            }

            unit = null;
            return false;
        }

        public bool TryGetUnit(int id, out AIUnitModel unit) => m_unitsById.TryGetValue(id, out unit);
        public bool TryGetUnit<T>(int id, out T unit) where T : AIUnitModel
        {
            if (TryGetUnit(id, out AIUnitModel aiUnitModel))
            {
                if (aiUnitModel is T typedUnit)
                {
                    unit = typedUnit;
                    return true;
                }
            }

            unit = null;
            return false;
        }

        public bool TryGetUnitPosition(int id, out Vector2 position) => m_unitPositions.TryGetValue(id, out position);

        public Board DeepCopy()
        {
            BoardGraph graph = Graph.DeepCopy();
            Board board = new Board(graph);
            
            foreach (var keyValuePair in m_units)
            {
                AIUnitModel model = keyValuePair.Value.DeepCopy();
                board.AddUnit(keyValuePair.Key, model);
            }

            return board;
        }

        public IEnumerable<AIUnitModel> GetUnits()
        {
            foreach (AIUnitModel unit in m_units.Values)
                yield return unit;
        }
    }
}
