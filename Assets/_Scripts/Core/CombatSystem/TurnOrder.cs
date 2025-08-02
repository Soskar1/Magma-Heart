using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrder
    {
        private LinkedList<ITurnController> m_turnOrder;
        private LinkedListNode<ITurnController> m_currentNode;

        public TurnOrder() => m_turnOrder = new LinkedList<ITurnController>();

        public ITurnController First => m_turnOrder.First.Value;

        public void Add(ITurnController turnController)
        {
            m_turnOrder.AddLast(turnController);

            if (m_currentNode == null)
                m_currentNode = m_turnOrder.First;
        }

        public void Remove(ITurnController entity)
        {
            if (m_currentNode.Value == entity)
                SetNext();

            m_turnOrder.Remove(entity);
        }

        public ITurnController Next()
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