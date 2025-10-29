namespace MagmaHeart.AI
{
    public class ActionNode : DecisionTreeNode
    {
        public Action Action { get; init; }

        public ActionNode(Action action) => Action = action;

        public override DecisionTreeNode MakeDecision() => this;
    }
}
