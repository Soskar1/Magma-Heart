namespace MagmaHeart.AI.Reasoning
{
    public class StrategyNode : DecisionTreeNode
    {
        public Strategy Strategy { get; init; }

        public StrategyNode(Strategy strategy) => Strategy = strategy;

        public override DecisionTreeNode MakeDecision() => this;
    }
}