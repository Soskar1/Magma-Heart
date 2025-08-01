using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrderBuilder
    {
        public TurnOrder Build(List<ITurnController> entities)
        {
            TurnOrder turnOrder = new TurnOrder();
            PriorityQueue<ITurnController, int> iniciativePriority = new PriorityQueue<ITurnController, int>();

            foreach (ITurnController entity in entities)
            {
                int iniciative = IniciativeRoll();
                iniciativePriority.Enqueue(entity, -iniciative);
            }

            while (iniciativePriority.Count > 0)
            {
                ITurnController entity = iniciativePriority.Dequeue();
                turnOrder.Add(entity);
            }

            return turnOrder;
        }

        private int IniciativeRoll() => Random.Range(1, 21);
    }
}