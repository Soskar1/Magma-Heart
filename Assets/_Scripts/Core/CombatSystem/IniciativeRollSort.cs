using System.Collections.Generic;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public static class IniciativeRollSort
    {
        public static IEnumerable<Entity> SortByRollingIniciative(List<Entity> entities)
        {
            PriorityQueue<Entity, int> iniciativePriority = new PriorityQueue<Entity, int>();

            foreach (Entity entity in entities)
            {
                int iniciative = IniciativeRoll();
                iniciativePriority.Enqueue(entity, -iniciative);
            }

            return iniciativePriority;
        }

        private static int IniciativeRoll() => Random.Range(1, 21);
    }
}