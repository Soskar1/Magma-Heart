using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly SortedDictionary<TPriority, Queue<TElement>> m_storage;
        public int Count { get; private set; }

        public PriorityQueue() 
        {
            m_storage = new SortedDictionary<TPriority, Queue<TElement>>();
            Count = 0;
        }

        public void Enqueue(TElement item, TPriority priority)
        {
            if (!m_storage.TryGetValue(priority, out var queue))
            {
                queue = new Queue<TElement>();
                m_storage[priority] = queue;
            }

            queue.Enqueue(item);
            ++Count;
        }

        public TElement Dequeue()
        {
            if (m_storage.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            var firstPair = m_storage.First();
            var item = firstPair.Value.Dequeue();
            --Count;

            if (firstPair.Value.Count == 0)
                m_storage.Remove(firstPair.Key);

            return item;
        }
    }
}