using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrderBuilder
    {
        public TurnOrder Build(List<ICombatController> entities)
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

        private int IniciativeRoll() => Random.Range(1, 21);
    }
}