namespace MagmaHeart.AI
{
    public class ActionNode : DecisionTreeNode
    {
        public IAction Action { get; init; }

        public ActionNode(IAction action) => Action = action;

        public override DecisionTreeNode MakeDecision() => this;
    }
}
