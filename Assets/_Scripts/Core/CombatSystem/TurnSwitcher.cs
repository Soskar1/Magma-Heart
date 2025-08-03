using System.Collections.Generic;
using MagmaHeart.Core.UI;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnSwitcher
    {
        private TurnOrder m_turnOrder;
        private ICombatController m_currentTurn;
        private List<IDisplayable> m_combatUI;

        public TurnSwitcher(TurnOrder turnOrder, List<IDisplayable> combatUI)
        {
            m_turnOrder = turnOrder;
            m_combatUI = combatUI;
            m_currentTurn = m_turnOrder.First;
        }

        public void Start()
        {
            m_currentTurn.NextTurn = NextTurn;
            StartTurn();
        }

        private void NextTurn()
        {
            EndTurn();

            m_currentTurn = m_turnOrder.Next();
            m_currentTurn.NextTurn = NextTurn;
            StartTurn();
        }

        private void StartTurn()
        {
            if (m_currentTurn.IsPlayableCharacter)
                foreach (IDisplayable ui in m_combatUI)
                    ui.Show();
            
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