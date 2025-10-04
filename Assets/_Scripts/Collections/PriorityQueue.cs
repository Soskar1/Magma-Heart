using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Collections
{
    public class PriorityQueue<TElement, TPriority> : IEnumerable<TElement> where TPriority : IComparable<TPriority>
    {
        private readonly SortedDictionary<TPriority, LinkedList<TElement>> m_storage;
        public int Count { get; private set; }

        public PriorityQueue() 
        {
            m_storage = new SortedDictionary<TPriority, LinkedList<TElement>>();
        }

        public void Enqueue(TElement item, TPriority priority)
        {
            if (!m_storage.TryGetValue(priority, out var queue))
            {
                queue = new LinkedList<TElement>();
                m_storage[priority] = queue;
            }

            queue.AddLast(item);
            ++Count;
        }

        public TElement Dequeue()
        {
            if (m_storage.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            (TPriority priority, LinkedList<TElement> linkedList) = m_storage.First();

            TElement item = linkedList.First.Value;
            linkedList.RemoveFirst();

            if (linkedList.Count == 0)
                m_storage.Remove(priority);

            --Count;
            return item;
        }

        public void Remove(TElement item)
        {
            foreach ((TPriority priority, LinkedList<TElement> linkedList) in m_storage)
            {
                LinkedListNode <TElement> node = linkedList.Find(item);
                if (node != null)
                {
                    linkedList.Remove(node);

                    if (linkedList.Count == 0)
                        m_storage.Remove(priority);

                    --Count;
                    return;
                }
            }
        }

        public void UpdatePriority(TElement item, TPriority newPriority)
        {
            Remove(item);
            Enqueue(item, newPriority);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (var keyValuePair in m_storage)
                foreach (TElement element in keyValuePair.Value)
                    yield return element;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}