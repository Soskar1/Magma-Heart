using MagmaHeart.Abilities.Effects;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Effects.Presenters
{
    public interface IEffectPresenter
    {
        public void Present(AbilityEffect effect);

        public void Hide();
    }

    public interface IEffectPresenter<TEffect> : IEffectPresenter where TEffect : AbilityEffect
    {
        public void Present(TEffect effect);

        void IEffectPresenter.Present(AbilityEffect effect)
        {
            if (effect is TEffect typedEffect)
            {
                Present(typedEffect);
            }
            else
            {
                Debug.LogWarning(
                    $"Invalid effect type. Expected {typeof(TEffect).Name}, got {effect?.GetType().Name}");
            }
        }
    }
}
