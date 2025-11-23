using System.Collections.Generic;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities.CombatSystem;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public static class IniciativeRollSort
    {
        public static IEnumerable<CombatController> SortByRollingIniciative(IEnumerable<CombatController> entities)
        {
            PriorityQueue<CombatController, int> iniciativePriority = new PriorityQueue<CombatController, int>();

            foreach (CombatController entity in entities)
            {
                int iniciative = IniciativeRoll();
                iniciativePriority.Enqueue(entity, -iniciative);
            }

            return iniciativePriority;
        }

        private static int IniciativeRoll() => Random.Range(1, 21);
    }
}