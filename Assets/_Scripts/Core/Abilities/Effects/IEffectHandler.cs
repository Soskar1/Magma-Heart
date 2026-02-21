using MagmaHeart.Abilities.Effects;

namespace MagmaHeart.Core.Abilities.Effects
{
    public interface IEffectHandler<in T> where T : AbilityEffect
    {
        public void Handle(T effect);
    }
}
