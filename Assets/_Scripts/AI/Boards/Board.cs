using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class Board
    {
        public BoardGraph Graph { get; init; }

        private Dictionary<Vector2, AIUnit> m_units;
        public IEnumerable<AIUnit> Units => m_units.Values;

        public Board(BoardGraph graph)
        {
            Graph = graph;
            m_units = new Dictionary<Vector2, AIUnit>();
        }

        public void ChangeNodeType(Vector2 position, BoardNodeType newNodeType) => Graph.ChangeNodeType(position, newNodeType);

        public void AddUnit(Vector2 position, AIUnit unit)
        {
            if (Graph.GetNode(position) == null)
                throw new ArgumentException($"Node at {position} does not exist in the board graph!");

            if (m_units.ContainsKey(position))
            {
                Debug.LogWarning($"There is already a unit at position {position}!");
                return;
            }

            m_units.Add(position, unit);
        }

        public bool RemoveUnit(Vector2 position) => m_units.Remove(position);

        public bool TryGetUnit(Vector2 position, out AIUnit unit) => m_units.TryGetValue(position, out unit);

        public Board DeepCopy()
        {
            BoardGraph graph = Graph.DeepCopy();
            Board board = new Board(graph);
            
            foreach (var keyValuePair in m_units)
                board.AddUnit(keyValuePair.Key, keyValuePair.Value);

            return board;
        }
    }
}
