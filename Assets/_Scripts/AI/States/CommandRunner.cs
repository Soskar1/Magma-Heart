using MagmaHeart.AI.Boards;
using System.Collections.Generic;

namespace MagmaHeart.AI.States
{
    public class CommandRunner
    {
        private Stack<IEnumerable<IBoardCommand>> m_history = new();
        internal Stack<IEnumerable<IBoardCommand>> History => m_history;

        public void Apply(Board board, IEnumerable<IBoardCommand> commands)
        {
            Stack<IBoardCommand> commandHistory = new();
            foreach (IBoardCommand command in commands)
            {
                command.Execute(board);
                commandHistory.Push(command);
            }
            
            m_history.Push(commandHistory);
        }

        public void Undo(Board board)
        {
            IEnumerable<IBoardCommand> commands = m_history.Pop();

            foreach (IBoardCommand command in commands)
                command.Undo(board);
        }
    }
}
