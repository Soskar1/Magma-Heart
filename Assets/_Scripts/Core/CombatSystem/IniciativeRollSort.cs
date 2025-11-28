using System.Collections.Generic;
using MagmaHeart.Collections;
using MagmaHeart.Core.Entities.Presenters;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public static class IniciativeRollSort
    {
        public static IEnumerable<EntityPresenter> SortByRollingIniciative(IEnumerable<EntityPresenter> entities)
        {
            PriorityQueue<EntityPresenter, int> iniciativePriority = new PriorityQueue<EntityPresenter, int>();

            foreach (EntityPresenter entity in entities)
            {
                int iniciative = IniciativeRoll();
                iniciativePriority.Enqueue(entity, -iniciative);
            }

            return iniciativePriority;
        }

        private static int IniciativeRoll() => Random.Range(1, 21);
    }
}