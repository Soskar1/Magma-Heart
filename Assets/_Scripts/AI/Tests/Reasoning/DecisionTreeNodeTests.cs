using MagmaHeart.AI.Boards;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public class DecisionTreeNodeTests
    {
        private class TestAction1 : Action
        {
            public TestAction1(AIUnit actionPossessor) : base(actionPossessor) { }

            public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, AIUnit target)
            {
                throw new System.NotImplementedException();
            }

            public override void Execute()
            {
                Debug.Log(nameof(TestAction1));
            }

            public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, AIUnit target)
            {
                throw new System.NotImplementedException();
            }
        }

        private class TestAction2 : Action
        {
            public TestAction2(AIUnit actionPossessor) : base(actionPossessor) { }

            public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, AIUnit target)
            {
                throw new System.NotImplementedException();
            }

            public override void Execute()
            {
                Debug.Log(nameof(TestAction2));
            }
        }

        private ActionNode m_first;
        private ActionNode m_second;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_first = new ActionNode(new TestAction1(null));
            m_second = new ActionNode(new TestAction2(null));
        }

        [Test]
        public void DecisionTreeNode_DecisionTreeCreation_MakeDecisionReturnsActionNode()
        {
            FloatDecision root = new FloatDecision(50, 79, () => 66);
            root.FalseNode = m_first;
            root.TrueNode = m_second;

            DecisionTreeNode node = root.MakeDecision();

            Assert.That(node is ActionNode, Is.True);
        }

        [Test]
        public void DecisionTreeNode_FloatDecision_ReturnsFalseNode()
        {
            FloatDecision root = new FloatDecision(50, 79, () => 99);
            root.FalseNode = m_first;
            root.TrueNode = m_second;

            ActionNode node = root.MakeDecision() as ActionNode;
            node.Action.Execute();

            LogAssert.Expect(nameof(TestAction1));
        }

        [Test]
        public void DecisionTreeNode_FloatDecision_ReturnsTrueNode()
        {
            FloatDecision root = new FloatDecision(50, 79, () => 65);
            root.FalseNode = m_first;
            root.TrueNode = m_second;

            ActionNode node = root.MakeDecision() as ActionNode;
            node.Action.Execute();

            LogAssert.Expect(nameof(TestAction2));
        }

        [Test]
        public void DecisionTreeNode_FloatDecisionChaining_ReturnsAction()
        {
            FloatDecision root = new FloatDecision(50, 79, () => 65);
            FloatDecision otherDecision = new FloatDecision(12, 87, () => 55);

            root.TrueNode = otherDecision;
            root.FalseNode = m_first;

            otherDecision.TrueNode = m_second;
            otherDecision.FalseNode = m_first;

            ActionNode node = root.MakeDecision() as ActionNode;
            node.Action.Execute();

            LogAssert.Expect(nameof(TestAction2));
        }
    }
}