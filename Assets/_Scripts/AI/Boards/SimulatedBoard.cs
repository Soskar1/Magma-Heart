using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public class SimulatedBoard
    {
        public BoardGraph Graph { get; init; }
        public Dictionary<Vector2, AIUnit> Units { get; init; }

        private Stack<BoardRecord> m_history;

        internal SimulatedBoard(BoardGraph graph, Dictionary<Vector2, AIUnit> units)
        {
            Graph = graph;
            m_history = new Stack<BoardRecord>();
            Units = new Dictionary<Vector2, AIUnit>(units);
        }

        public void ApplyBoardModification(int simulationDepth, BoardModification boardModification)
        {
            BoardRecord record = new BoardRecord(simulationDepth, boardModification);
            m_history.Push(record);

            boardModification.Apply(this);
        }

        internal void UndoBoardModification(int simulationDepth)
        {
            while (m_history.Count > 0 && m_history.Peek().SimulationDepth == simulationDepth)
            {
                BoardRecord record = m_history.Pop();
                record.BoardModification.Undo(this);
            }
        }

        public bool TryGetUnitOnPosition(Vector2 position, out AIUnit unit)
        {
            if (Units.TryGetValue(position, out unit))
                return true;

            return false;
        }

        public bool IsBoardNodeEmpty(Vector2 position)
        {
            if (Units.ContainsKey(position))
                return false;

            BoardNode node = Graph.GetNode(position);
            if (node.Type == BoardNodeType.Walkable)
                return true;

            return false;
        }
    }
}
