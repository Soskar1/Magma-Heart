namespace MagmaHeart.AI
{
    public abstract class DecisionNode : DecisionTreeNode
    {
        public DecisionTreeNode FalseNode { get; set; }
        public DecisionTreeNode TrueNode { get; set; }

        public abstract DecisionTreeNode GetBranch();

        public override DecisionTreeNode MakeDecision()
        {
            DecisionTreeNode branch = GetBranch();
            return branch.MakeDecision();
        }
    }
}
