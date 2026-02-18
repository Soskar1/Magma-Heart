using MagmaHeart.AI.Boards;
using System.Collections.Generic;

namespace MagmaHeart.AI.Execution
{
    public class CommandRunner
    {
        private readonly Stack<List<IBoardCommand>> m_history = new();
        private readonly bool m_useMemory;

        internal Stack<List<IBoardCommand>> History => m_history;

        public CommandRunner(bool useMemory = true) => m_useMemory = useMemory;

        public void Apply(Board board, IEnumerable<IBoardCommand> commands)
        {
            var executed = new List<IBoardCommand>();

            foreach (IBoardCommand command in commands)
            {
                command.Execute(board);
                executed.Add(command);
            }

            if (m_useMemory)
                m_history.Push(executed);
        }

        public void Apply(Board board, IBoardCommand command)
        {
            command.Execute(board);
            
            if (m_useMemory)
                m_history.Push(new List<IBoardCommand> { command });
        }

        public void Undo(Board board)
        {
            if (!m_useMemory)
                return;

            if (m_history.Count == 0)
                return;

            var executed = m_history.Pop();

            for (int i = executed.Count - 1; i >= 0; --i)
                executed[i].Undo(board);
        }
    }
}
