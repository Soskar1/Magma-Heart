using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record MoveCommand(int ExecutorId, Vector2 From, Vector2 To) : IBoardCommand
    {
        public void Execute(Board board)
        {
            board.TryGetUnit(ExecutorId, out Entity Executor);

            Executor.Position = To;
            board.RemoveUnit(From);
            board.AddUnit(To, Executor);
        }

        public void Undo(Board board)
        {
            board.TryGetUnit(ExecutorId, out Entity Executor);

            Executor.Position = From;
            board.RemoveUnit(To);
            board.AddUnit(From, Executor);
        }
    }
}
