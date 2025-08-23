using System.Collections.Generic;
using MagmaHeart.Core.Collections;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public static class TurnOrderBuilder
    {
        public static TurnOrder Build(List<ICombatController> entities)
        {
            TurnOrder turnOrder = new TurnOrder();
            PriorityQueue<ICombatController, int> iniciativePriority = new PriorityQueue<ICombatController, int>();

            foreach (ICombatController entity in entities)
            {
                int iniciative = IniciativeRoll();
                iniciativePriority.Enqueue(entity, -iniciative);
            }

            while (iniciativePriority.Count > 0)
            {
                ICombatController entity = iniciativePriority.Dequeue();
                turnOrder.Add(entity);
            }

            return turnOrder;
        }

        private static int IniciativeRoll() => Random.Range(1, 21);
    }
}