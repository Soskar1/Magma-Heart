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

        public void ApplyBoardModification(Action action, BoardModification boardModification)
        {
            BoardRecord record = new BoardRecord(action, boardModification);
            m_history.Push(record);

            boardModification.Apply(this);
        }

        internal void UndoBoardModification(Action action)
        {
            if (m_history.Count > 0 && m_history.Peek().Action == action)
            {
                BoardRecord record = m_history.Pop();
                record.boardModification.Undo(this);
            }
        }

        public bool TryGetUnitOnPosition(Vector2 position, out AIUnit unit)
        {
            if (Units.TryGetValue(position, out unit))
                return true;

            return false;
        }
    }
}
