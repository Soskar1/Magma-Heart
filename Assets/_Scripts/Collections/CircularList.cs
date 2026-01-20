using System.Collections;
using System.Collections.Generic;

namespace MagmaHeart.Collections
{
    public class CircularList<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> m_list;
        private LinkedListNode<T> m_currentNode;

        public int Count => m_list.Count;

        public T Head => m_currentNode.Value;

        public CircularList() => m_list = new LinkedList<T>();

        public void Add(T item)
        {
            m_list.AddLast(item);

            if (m_currentNode == null)
                m_currentNode = m_list.First;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public bool Remove(T item)
        {
            if (m_currentNode.Value.Equals(item))
                SetNext();

            return m_list.Remove(item);
        }

        public T Next()
        {
            SetNext();
            return m_currentNode.Value;
        }

        public T NextTo(T item)
        {
            LinkedListNode<T> node = m_list.Find(item);
            
            if (node.Next == null)
                return m_list.First.Value;

            return node.Next.Value;
        }

        private void SetNext()
        {
            if (m_currentNode.Next == null)
                m_currentNode = m_list.First;
            else
                m_currentNode = m_currentNode.Next;
        }

        public void Clear()
        {
            m_list.Clear();
            m_currentNode = null;
        }

        public bool Contains(T item) => m_list.Contains(item);

        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> tmpNode = m_currentNode;

            do
            {
                yield return tmpNode.Value;
                tmpNode = tmpNode.Next;

                if (tmpNode == null)
                    tmpNode = m_list.First;

            } while (tmpNode != m_currentNode);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}