using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrder
    {
        private LinkedList<ICombatController> m_turnOrder;
        private LinkedListNode<ICombatController> m_currentNode;

        public TurnOrder() => m_turnOrder = new LinkedList<ICombatController>();

        public ICombatController First => m_turnOrder.First.Value;

        public void Add(ICombatController turnController)
        {
            m_turnOrder.AddLast(turnController);

            if (m_currentNode == null)
                m_currentNode = m_turnOrder.First;
        }

        public void Remove(ICombatController entity)
        {
            if (m_currentNode.Value == entity)
                SetNext();

            m_turnOrder.Remove(entity);
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