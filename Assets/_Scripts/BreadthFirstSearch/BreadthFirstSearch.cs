using System;
using System.Collections.Generic;

namespace MagmaHeart.BreadthFirstSearch
{
    public class BreadthFirstSearch<T> : IBreadthFirstSearch<T>
    {
        private readonly Func<T, IEnumerable<T>> m_getNeighbours;
        private readonly Predicate<T> m_neighbourAcceptanceCriteria;
        private readonly Action<T, T> m_onNeighbourFound;

        public BreadthFirstSearch(Func<T, IEnumerable<T>> getNeighbours, Action<T, T> onNeighbourFound = null)
            : this(getNeighbours, _ => true, onNeighbourFound) { }

        public BreadthFirstSearch(Func<T, IEnumerable<T>> getNeighbours, Predicate<T> neighbourAcceptanceCriteria, Action<T, T> onNeighbourFound = null)
        {
            m_getNeighbours = getNeighbours;
            m_neighbourAcceptanceCriteria = neighbourAcceptanceCriteria;
            m_onNeighbourFound = onNeighbourFound;
        }

        public IEnumerable<T> Perform(T firstElement)
        {
            HashSet<T> visited = new HashSet<T>();
            Queue<T> queue = new Queue<T>();
            queue.Enqueue(firstElement);

            while (queue.Count > 0)
            {
                T element = queue.Dequeue();
                visited.Add(element);

                IEnumerable<T> neighbours = m_getNeighbours(element);
                foreach (T neighbour in neighbours)
                {
                    if (m_neighbourAcceptanceCriteria(neighbour) && !visited.Contains(neighbour) && !queue.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        m_onNeighbourFound?.Invoke(element, neighbour);
                    }
                }
            }

            return visited;
        }
    }
}