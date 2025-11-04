using System;
using System.Collections.Generic;

namespace MagmaHeart.Collections
{
    public class TypeMap<TBase>
    {
        private readonly Dictionary<Type, TBase> m_items = new();

        public TypeMap() { }
        public TypeMap(Dictionary<Type, TBase> items) => m_items = new Dictionary<Type, TBase>(items);

        public void Add<T>(T item) where T : TBase
            => m_items[typeof(T)] = item;

        public T Get<T>() where T : TBase
            => (T)m_items[typeof(T)];

        public bool TryGet<T>(out T item) where T : TBase
        {
            if (m_items.TryGetValue(typeof(T), out TBase value))
            {
                item = (T)value;
                return true;
            }

            item = default;
            return false;
        }

        public void Remove<T>() where T : TBase
            => m_items.Remove(typeof(T));

        public bool Contains<T>() where T : TBase
            => m_items.ContainsKey(typeof(T));

        public TypeMap<TBase> DeepCopy(Func<TBase, TBase> cloneFunc = null)
        {
            var copy = new TypeMap<TBase>();
            foreach (var kvp in m_items)
                copy.m_items[kvp.Key] = cloneFunc != null ? cloneFunc(kvp.Value) : kvp.Value;

            return copy;
        }
    }
}
