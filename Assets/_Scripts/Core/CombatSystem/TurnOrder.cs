using MagmaHeart.Collections;
using MagmaHeart.Core.Entities;
using System.Collections;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrder : IEnumerable<Entity>
    {
        private readonly CircularList<Entity> m_turnOrder;
        public Entity Current => m_turnOrder.Head;

        public TurnOrder(IEnumerable<Entity> entities)
        {
            m_turnOrder = new CircularList<Entity>();
            m_turnOrder.AddRange(entities);
        }

        public void Next() => m_turnOrder.Next();
        public void Remove(Entity context) => m_turnOrder.Remove(context);
        public void Clear() => m_turnOrder.Clear();

        public IEnumerator<Entity> GetEnumerator() => m_turnOrder.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
