using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrder
    {
        private LinkedList<ICombatController> m_turnOrder;
        private LinkedListNode<ICombatController> m_currentNode;

        public TurnOrder() => m_turnOrder = new LinkedList<ICombatController>();

        public ICombatController First => m_turnOrder.First.Value;

        public void Add(ICombatController combatController)
        {
            m_turnOrder.AddLast(combatController);

            if (m_currentNode == null)
                m_currentNode = m_turnOrder.First;
        }

        public void AddRange(IEnumerable<ICombatController> combatControllers)
        {
            foreach (var combatController in combatControllers)
                Add(combatController);
        }

        public void Remove(ICombatController entity)
        {
            if (m_currentNode.Value == entity)
                SetNext();

            m_turnOrder.Remove(entity);
        }

        public void Clear()
        {
            m_turnOrder.Clear();
            m_currentNode = null;
        }

        public ICombatController Next()
        {
            SetNext();

            return m_currentNode.Value;
        }

        private void SetNext()
        {
            if (m_currentNode.Next == null)
                m_currentNode = m_turnOrder.First;
            else
                m_currentNode = m_currentNode.Next;
        }
    }
}