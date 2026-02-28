using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;

namespace MagmaHeart.Core.Abilities.Effects
{
    public interface IEffectHandler<in T> where T : AbilityEffect
    {
        public void Handle(IBoardGameWorld world, T effect);
    }
}
