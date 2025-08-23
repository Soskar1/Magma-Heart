using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly SortedDictionary<TPriority, LinkedList<TElement>> m_storage;
        private readonly HashSet<TElement> m_elements;
        public int Count => m_elements.Count;

        public PriorityQueue() 
        {
            m_storage = new SortedDictionary<TPriority, LinkedList<TElement>>();
            m_elements = new HashSet<TElement>();
        }

        public void Enqueue(TElement item, TPriority priority)
        {
            if (m_elements.Contains(item))
                return;

            if (!m_storage.TryGetValue(priority, out var queue))
            {
                queue = new LinkedList<TElement>();
                m_storage[priority] = queue;
            }

            queue.AddLast(item);
            m_elements.Add(item);
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

            m_elements.Remove(item);
            return item;
        }

        public void Remove(TElement item)
        {
            if (!m_elements.Contains(item))
                return;

            foreach (LinkedList<TElement> linkedList in m_storage.Values)
            {
                LinkedListNode<TElement> node = linkedList.Find(item);
                if (node != null)
                {
                    linkedList.Remove(node);
                    m_elements.Remove(item);
                    return;
                }
            }
        }

        public void UpdatePriority(TElement item, TPriority newPriority)
        {
            if (!m_elements.Contains(item))
                return;

            Remove(item);
            Enqueue(item, newPriority);
        }
    }
}