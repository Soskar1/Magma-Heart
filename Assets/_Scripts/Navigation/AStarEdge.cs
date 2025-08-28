using MagmaHeart.Extensions;

namespace MagmaHeart.Navigation
{
    public class AStarEdge
    {
        public float Cost { get; set; }
        public AStarNode First { get; private set; }
        public AStarNode Second { get; private set; }

        public AStarEdge(AStarNode first, AStarNode second, float cost)
        {
            first.ThrowIfNull(nameof(first));
            second.ThrowIfNull(nameof(second));

            First = first;
            Second = second;
            Cost = cost;
        }
    }
}