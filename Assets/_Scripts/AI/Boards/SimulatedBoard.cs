using System.Collections.Generic;

namespace MagmaHeart.AI.Boards
{
    public class SimulatedBoard
    {
        internal BoardGraph Graph { get; init; }

        private Stack<BoardRecord> m_history;

        internal SimulatedBoard(BoardGraph graph)
        {
            Graph = graph;
            m_history = new Stack<BoardRecord>();
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
    }
}
