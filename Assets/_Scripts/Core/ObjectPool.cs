using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private Queue<T> m_pool = new Queue<T>();
        private Action<T> m_initialization;
        private T m_prefab;

        public ObjectPool(T prefab, Action<T> initialization, int initialSize = 0)
        {
            m_prefab = prefab;
            m_initialization = initialization;

            for (int i = 0; i < initialSize; i++)
            {
                T obj = GameObject.Instantiate(m_prefab);
                m_initialization(obj);
                PushToPool(obj);
                obj.gameObject.SetActive(false);
            }
        }

        public T Get()
        {
            T obj;

            if (m_pool.Count > 0)
            {
                obj = m_pool.Dequeue();
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = GameObject.Instantiate(m_prefab);
                m_initialization(obj);
            }

            obj.OnSpawn();
            return obj;
        }

        public void PushToPool(T obj)
        {
            obj.OnReturnToPool();
            m_pool.Enqueue(obj);
        }
    }
}