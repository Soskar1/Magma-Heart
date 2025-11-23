using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnOrder
    {
        private readonly CircularList<TurnContext> m_turnOrder;
        public TurnContext Current => m_turnOrder.Head;

        public TurnOrder(IEnumerable<TurnContext> turnContexts)
        {
            m_turnOrder = new CircularList<TurnContext>();
            m_turnOrder.AddRange(turnContexts);
        }

        public void Next() => m_turnOrder.Next();
        public void Remove(TurnContext context) => m_turnOrder.Remove(context);
        public void Clear() => m_turnOrder.Clear();

        public ChainNode<TurnContext> ToChainNode() => (ChainNode<TurnContext>)m_turnOrder;
    }
}
