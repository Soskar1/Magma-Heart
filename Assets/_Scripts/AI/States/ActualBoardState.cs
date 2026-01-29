using MagmaHeart.AI.Boards;

namespace MagmaHeart.AI.States
{
    public abstract class ActualBoardState : BoardState
    {
        protected ActualBoardState(Board board) : base(board) { }
    }
}