using System.Collections.Generic;
using System;
using UnityEngine;

namespace MagmaHeart.Core.ObjectPooling
{
    public class ObjectPoolMap
    {
        private Dictionary<Type, object> pools = new Dictionary<Type, object>();

        public void RegisterPool<T>(T prefab, Action<T> initialization, int initialSize = 0) where T : MonoBehaviour, IPoolable
        {
            ObjectPool<T> pool = new ObjectPool<T>(prefab, initialization, initialSize);
            pools[prefab.GetType()] = pool;
        }

        public bool IsRegistered(Type type) => pools.ContainsKey(type);

        public T Get<T>(Type type) where T : MonoBehaviour, IPoolable
        {
            if (pools.TryGetValue(type, out object poolObj) && poolObj is ObjectPool<T> pool)
                return pool.Get();
            
            return null;
        }

        public void Return<T>(T obj) where T : MonoBehaviour, IPoolable
        {
            if (pools.TryGetValue(obj.GetType(), out object poolObj) && poolObj is ObjectPool<T> pool)
                pool.PushToPool(obj);
        }
    }
}