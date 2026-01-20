using System.Collections.Generic;

namespace MagmaHeart.BreadthFirstSearch
{
    public interface IBreadthFirstSearch<T>
    {
        IEnumerable<T> Perform(T firstElement);
    }
}
