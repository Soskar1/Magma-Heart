using MagmaHeart.Abilities.Effects;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public interface IEffectPresenter
    {
        public void Present(GameWorld world, AbilityEffect effect);
    }

    public interface IEffectPresenter<TEffect> : IEffectPresenter where TEffect : AbilityEffect
    {
        public void Present(GameWorld world, TEffect effect);

        void IEffectPresenter.Present(GameWorld world, AbilityEffect effect)
        {
            if (effect is TEffect typedEffect)
            {
                Present(world, typedEffect);
            }
            else
            {
                Debug.LogWarning(
                    $"Invalid effect type. Expected {typeof(TEffect).Name}, got {effect?.GetType().Name}");
            }
        }
    }
}
