using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class Board
    {
        public BoardGraph Graph { get; init; }

        private Dictionary<Vector2, HashSet<AIUnitModel>> m_units;

        public Board(BoardGraph graph)
        {
            Graph = graph;
            m_units = new Dictionary<Vector2, HashSet<AIUnitModel>>();
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => Graph.ChangeNodeType(position, newNodeType);
        public BoardNodeType GetNodeType(Vector2 position) => Graph.GetNode(position).Type;

        public void AddUnit(Vector2 position, AIUnitModel unit)
        {
            if (Graph.GetNode(position) == null)
                throw new ArgumentException($"Node at {position} does not exist in the board graph!");

            if (!m_units.ContainsKey(position))
                m_units[position] = new HashSet<AIUnitModel>();

            m_units[position].Add(unit);
        }

        public bool RemoveUnit(Vector2 position, AIUnitModel unit)
        {
            if (!m_units.ContainsKey(position))
                return false;

            bool result = m_units[position].Remove(unit);
            
            if (result && !m_units[position].Any())
                return m_units.Remove(position);

            return result;
        }

        public bool TryGetUnits(Vector2 position, out HashSet<AIUnitModel> units) => m_units.TryGetValue(position, out units);

        public Board DeepCopy()
        {
            BoardGraph graph = Graph.DeepCopy();
            Board board = new Board(graph);
            
            foreach (var keyValuePair in m_units)
                foreach (AIUnitModel unit in keyValuePair.Value)
                    board.AddUnit(keyValuePair.Key, unit);

            return board;
        }

        public IEnumerable<AIUnitModel> GetUnits()
        {
            foreach (HashSet<AIUnitModel> hashSet in m_units.Values)
                foreach (AIUnitModel unit in hashSet)
                    yield return unit;
        }
    }
}
