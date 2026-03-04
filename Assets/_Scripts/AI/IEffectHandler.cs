using MagmaHeart.Abilities.Effects;

namespace MagmaHeart.AI
{
    public interface IEffectHandler<in T> where T : AbilityEffect
    {
        public void Handle(IBoardGameWorld world, T effect);
    }
}
