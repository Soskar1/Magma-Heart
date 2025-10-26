using System.Linq;

namespace MagmaHeart.AI.Reasoning
{
    public record StateSnapshot(PropertySnapshot[] PropertySnapshots)
    {
        public float StaticEvalution() => PropertySnapshots.Sum(x => x.Value * x.Weight);
    }
}
