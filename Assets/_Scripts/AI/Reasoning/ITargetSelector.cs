using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Targeting;

namespace MagmaHeart.AI.Reasoning
{
    public interface ITargetSelector
    {
        public AbilityTarget SelectTarget(IGameWorld world, int executorId);
    }
}
