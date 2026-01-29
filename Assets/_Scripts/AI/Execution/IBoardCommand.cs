using MagmaHeart.AI.Boards;

namespace MagmaHeart.AI.Execution
{
    public interface IBoardCommand
    {
        int ExecutorId { get; }
        public void Execute(Board board);
        public void Undo(Board board);
    }
}
