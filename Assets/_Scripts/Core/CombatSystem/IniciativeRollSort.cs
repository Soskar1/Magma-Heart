using System.Collections.Generic;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public static class IniciativeRollSort
    {
        public static IEnumerable<Entity> SortByRollingIniciative(IEnumerable<Entity> entities)
        {
            Comparer<int> inverseComparer = Comparer<int>.Create((a, b) => 0 - a.CompareTo(b));
            PriorityQueue<Entity, int> queue = new PriorityQueue<Entity, int>(inverseComparer);

            foreach (Entity entity in entities)
            {
                int iniciative = IniciativeRoll();
                queue.Enqueue(entity, iniciative);
            }

            List<Entity> sortedEntities = new List<Entity>();
            while (queue.Count > 0)
                sortedEntities.Add(queue.Dequeue());

            return sortedEntities;
        }

        private static int IniciativeRoll() => Random.Range(1, 21);
    }
}