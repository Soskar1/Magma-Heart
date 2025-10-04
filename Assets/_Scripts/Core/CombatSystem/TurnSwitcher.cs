using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnSwitcher
    {
        public TurnOrder TurnOrder { get; init; }

        private ICombatController m_currentTurn;

        public EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;

        public TurnSwitcher() => TurnOrder = new TurnOrder();

        public void Start(IEnumerable<ICombatController> entities)
        {
            TurnOrder.Clear();
            TurnOrder.AddRange(entities);

            m_currentTurn = TurnOrder.First;
            m_currentTurn.NextTurn = NextTurn;
            StartTurn();
        }

        private void NextTurn(object obj, EventArgs e)
        {
            EndTurn();

            m_currentTurn = TurnOrder.Next();
            m_currentTurn.NextTurn = NextTurn;
            StartTurn();
        }

        private void StartTurn()
        {
            OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(m_currentTurn);
            OnTurnSwitched?.Invoke(this, args);

            m_currentTurn.StartTurn();
        }

        private void EndTurn()
        {
            m_currentTurn.NextTurn = null;
        }
    }
}