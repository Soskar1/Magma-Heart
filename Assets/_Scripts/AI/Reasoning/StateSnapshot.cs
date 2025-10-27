using System.Linq;

namespace MagmaHeart.AI.Reasoning
{
    public record StateSnapshot(PropertySnapshot[] PropertySnapshots, bool IsGameOver)
    {
        public float StaticEvaluation() => PropertySnapshots.Sum(x => x.Value * x.Weight);
    }
}
