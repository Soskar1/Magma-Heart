using MagmaHeart.Abilities.Targeting;

namespace MagmaHeart.AI.Reasoning
{
    public interface ITargetSelector
    {
        public AbilityTarget SelectTarget(IBoardGameWorld world, int executorId);
    }
}
