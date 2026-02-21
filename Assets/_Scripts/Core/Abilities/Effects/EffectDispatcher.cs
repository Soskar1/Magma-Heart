using MagmaHeart.Abilities.Effects;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.Abilities.Effects
{
    public sealed class EffectDispatcher
    {
        private readonly Dictionary<Type, List<IUntypedHandler>> m_handlers = new();

        public void Register<T>(IEffectHandler<T> handler) where T : AbilityEffect
        {
            Type key = typeof(T);

            if (!m_handlers.TryGetValue(key, out List<IUntypedHandler> list))
            {
                list = new List<IUntypedHandler>();
                m_handlers[key] = list;
            }

            list.Add(new UntypedHandler<T>(handler));
        }

        public void Apply(AbilityEffect effect)
        {
            if (effect == null) return;

            Type type = effect.GetType();
            if (m_handlers.TryGetValue(type, out List<IUntypedHandler> list))
            {
                for (int i = 0; i < list.Count; i++)
                    list[i].Handle(effect);
            }
            else
            {
                // Optional: log for debugging
                // UnityEngine.Debug.LogWarning($"No handler registered for {type.Name}");
            }
        }

        private interface IUntypedHandler
        {
            void Handle(AbilityEffect effect);
        }

        private sealed class UntypedHandler<T> : IUntypedHandler where T : AbilityEffect
        {
            private readonly IEffectHandler<T> m_handler;

            public UntypedHandler(IEffectHandler<T> handler) => m_handler = handler;

            public void Handle(AbilityEffect effect) => m_handler.Handle((T)effect);
        }
    }
}
