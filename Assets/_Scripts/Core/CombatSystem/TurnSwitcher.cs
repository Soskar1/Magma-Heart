namespace MagmaHeart.Core.CombatSystem
{
    public class TurnSwitcher
    {
        private TurnOrder m_turnOrder;
        private ICombatController m_currentTurn;

        public TurnSwitcher(TurnOrder turnOrder)
        {
            m_turnOrder = turnOrder;
            m_currentTurn = m_turnOrder.First;
        }

        public void Start()
        {
            m_currentTurn.NextTurn = NextTurn;
            m_currentTurn.StartTurn();
        }

        private void NextTurn()
        {
            m_currentTurn.NextTurn = null;
            m_currentTurn = m_turnOrder.Next();
            m_currentTurn.NextTurn = NextTurn;
            m_currentTurn.StartTurn();
        }
    }
}