using System;
using System.Collections.Generic;

namespace MagmaHeart.BreadthFirstSearch
{
    public class BreadthFirstSearch<T> : IBreadthFirstSearch<T>
    {
        private readonly Func<T, IEnumerable<T>> m_getNeighbours;
        private readonly Predicate<T> m_neighbourAcceptanceCriteria;
        private readonly Action<T, T> m_onNonVisitedNeighbourIterated;

        public BreadthFirstSearch(Func<T, IEnumerable<T>> getNeighbours, Action<T, T> onNonVisitedNeighbourIterated = null)
            : this(getNeighbours, _ => true, onNonVisitedNeighbourIterated) { }

        public BreadthFirstSearch(Func<T, IEnumerable<T>> getNeighbours, Predicate<T> neighbourAcceptanceCriteria, Action<T, T> onNonVistedNeigbourIterated = null)
        {
            m_getNeighbours = getNeighbours;
            m_neighbourAcceptanceCriteria = neighbourAcceptanceCriteria;
            m_onNonVisitedNeighbourIterated = onNonVistedNeigbourIterated;
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
                    if (!visited.Contains(neighbour))
                    {
                        m_onNonVisitedNeighbourIterated?.Invoke(element, neighbour);

                        if (m_neighbourAcceptanceCriteria(neighbour) && !queue.Contains(neighbour))
                            queue.Enqueue(neighbour);
                    }
                }
            }

            return visited;
        }
    }
}