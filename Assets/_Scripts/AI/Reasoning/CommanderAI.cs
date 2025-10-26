namespace MagmaHeart.AI.Reasoning
{
    public class CommanderAI
    {
        private readonly DecisionTreeNode m_decisionTreeRoot;
        private readonly TacticianAI m_tacticianAI;

        public CommanderAI(DecisionTreeNode root)
        {
            m_tacticianAI = new TacticianAI();
            m_decisionTreeRoot = root;

            DecideWithStrategy();
        }

        public void DecideWithStrategy()
        {
            StrategyNode node = m_decisionTreeRoot.MakeDecision() as StrategyNode;

            m_tacticianAI.CurrentStrategy = node.Strategy;
        }
    }
}