using System;
using System.Collections.Generic;
using MagmaHeart.Core.UI;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnSwitcher
    {
        public TurnOrder TurnOrder { get; init; }

        private ICombatController m_currentTurn;
        private List<IDisplayable> m_combatUI;

        public EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;

        public TurnSwitcher(TurnOrder turnOrder, List<IDisplayable> combatUI)
        {
            TurnOrder = turnOrder;
            m_combatUI = combatUI;
            m_currentTurn = TurnOrder.First;
        }

        public void Start()
        {
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
            if (m_currentTurn.IsPlayableCharacter)
                foreach (IDisplayable ui in m_combatUI)
                    ui.Show();

            OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(m_currentTurn);
            OnTurnSwitched?.Invoke(this, args);

            m_currentTurn.StartTurn();
        }

        private void EndTurn()
        {
            if (m_currentTurn.IsPlayableCharacter)
                foreach (IDisplayable ui in m_combatUI)
                    ui.Hide();

            m_currentTurn.NextTurn = null;
        }
    }
}