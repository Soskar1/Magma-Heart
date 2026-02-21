using MagmaHeart.Abilities.Targeting;

namespace MagmaHeart.Abilities.Requirements
{
    public interface IAbilityRequirement
    {
        public abstract bool IsMet(IGameWorld gameWorld, int executorId, AbilityTarget target);
    }
}
