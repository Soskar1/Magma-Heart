using MagmaHeart.Extensions;

namespace MagmaHeart.AI.Boards
{
    internal class BoardEdge
    {
        public float Cost { get; set; }
        public BoardNode First { get; private set; }
        public BoardNode Second { get; private set; }

        public BoardEdge(BoardNode first, BoardNode second, float cost)
        {
            first.ThrowIfNull(nameof(first));
            second.ThrowIfNull(nameof(second));

            First = first;
            Second = second;
            Cost = cost;
        }
    }
}