using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class Board
    {
        public BoardGraph Graph { get; init; }

        private Dictionary<Vector2, AIUnitModel> m_units;

        public Board(BoardGraph graph)
        {
            Graph = graph;
            m_units = new Dictionary<Vector2, AIUnitModel>();
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => Graph.ChangeNodeType(position, newNodeType);
        public BoardNodeType GetNodeType(Vector2 position) => Graph.GetNode(position).Type;

        public void AddUnit(Vector2 position, AIUnitModel unit)
        {
            if (Graph.GetNode(position) == null)
                throw new ArgumentException($"Node at {position} does not exist in the board graph!");

            m_units[position] = unit;
        }

        public bool RemoveUnit(Vector2 position, AIUnitModel unit)
        {
            if (!m_units.ContainsKey(position))
                return false;

            return m_units.Remove(position);
        }

        public bool TryGetUnit(Vector2 position, out AIUnitModel units) => m_units.TryGetValue(position, out units);

        public Board DeepCopy()
        {
            BoardGraph graph = Graph.DeepCopy();
            Board board = new Board(graph);
            
            foreach (var keyValuePair in m_units)
                board.AddUnit(keyValuePair.Key, keyValuePair.Value);

            return board;
        }

        public IEnumerable<AIUnitModel> GetUnits()
        {
            foreach (AIUnitModel unit in m_units.Values)
                yield return unit;
        }
    }
}
