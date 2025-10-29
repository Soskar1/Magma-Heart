using MagmaHeart.AI.Reasoning;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MagmaHeart.AI.Tests
{
    public class DecisionTreeNodeTests
    {
        private class TestAction1 : IAction
        {
            public AIUnit ActionPossessor => throw new System.NotImplementedException();

            public bool CanSimulate(StateSnapshot state, AIUnit target)
            {
                throw new System.NotImplementedException();
            }

            public void Execute()
            {
                Debug.Log(nameof(TestAction1));
            }

            public StateSnapshot Simulate(StateSnapshot state)
            {
                throw new System.NotImplementedException();
            }

            public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
            {
                throw new System.NotImplementedException();
            }
        }

        private class TestAction2 : IAction
        {
            public AIUnit ActionPossessor => throw new System.NotImplementedException();

            public bool CanSimulate(StateSnapshot state, AIUnit target)
            {
                throw new System.NotImplementedException();
            }

            public void Execute()
            {
                Debug.Log(nameof(TestAction2));
            }

            public StateSnapshot Simulate(StateSnapshot state)
            {
                throw new System.NotImplementedException();
            }

            public StateSnapshot Simulate(StateSnapshot state, AIUnit target)
            {
                throw new System.NotImplementedException();
            }
        }

        private ActionNode m_first;
        private ActionNode m_second;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_first = new ActionNode(new TestAction1());
            m_second = new ActionNode(new TestAction2());
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