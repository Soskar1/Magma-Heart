using System.Collections.Generic;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrderBuilder
    {
        public LinkedList<Entity> Build(List<Entity> entities)
        {
            LinkedList<Entity> turnOrder = new LinkedList<Entity>();
            PriorityQueue<Entity, int> iniciativePriority = new PriorityQueue<Entity, int>();

            foreach (Entity entity in entities)
            {
                int iniciative = IniciativeRoll();
                iniciativePriority.Enqueue(entity, -iniciative);
            }

            while (iniciativePriority.Count > 0)
            {
                Entity entity = iniciativePriority.Dequeue();
                turnOrder.AddLast(entity);
            }

            return turnOrder;
        }

        private int IniciativeRoll() => Random.Range(1, 21);
    }
}