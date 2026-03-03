using MagmaHeart.Abilities.Targeting;

namespace MagmaHeart.Abilities.Requirements
{
    public interface IAbilityRequirement
    {
        public abstract bool IsMet(IGameWorld gameWorld, int executorId, AbilityTarget target);
    }

    public interface IAbilityRequirement<T> : IAbilityRequirement where T : IGameWorld
    {
        public abstract bool IsMet(T gameWorld, int executorId, AbilityTarget target);

        bool IAbilityRequirement.IsMet(IGameWorld gameWorld, int executorId, AbilityTarget target) => IsMet((T)gameWorld, executorId, target);
    }
}
