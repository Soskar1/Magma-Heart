using System.Collections.Generic;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> ts)
    {
        int count = ts.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int rng = UnityEngine.Random.Range(i, count);
            T tmp = ts[i];
            ts[i] = ts[rng];
            ts[rng] = tmp;
        }
    }
}
