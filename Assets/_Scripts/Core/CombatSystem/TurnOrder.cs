using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrder
    {
        private CircularList<ICombatController> m_turnOrder;

        public TurnOrder() => m_turnOrder = new CircularList<ICombatController>();

        public ICombatController First => m_turnOrder.Head;

        public void Add(ICombatController combatController) => m_turnOrder.Add(combatController);

        public void AddRange(IEnumerable<ICombatController> combatControllers) => m_turnOrder.AddRange(combatControllers);

        public void Remove(ICombatController entity) => m_turnOrder.Remove(entity);

        public void Clear() => m_turnOrder.Clear();

        public ICombatController Next() => m_turnOrder.Next();
    }
}