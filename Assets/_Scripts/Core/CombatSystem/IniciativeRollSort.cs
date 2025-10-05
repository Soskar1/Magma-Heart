using System.Collections.Generic;
using MagmaHeart.Collections;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public static class IniciativeRollSort
    {
        public static IEnumerable<ICombatController> SortByRollingIniciative(List<ICombatController> entities)
        {
            TurnOrder turnOrder = new TurnOrder();
            PriorityQueue<ICombatController, int> iniciativePriority = new PriorityQueue<ICombatController, int>();

            foreach (ICombatController entity in entities)
            {
                int iniciative = IniciativeRoll();
                iniciativePriority.Enqueue(entity, -iniciative);
            }

            return iniciativePriority;
        }

        private static int IniciativeRoll() => Random.Range(1, 21);
    }
}