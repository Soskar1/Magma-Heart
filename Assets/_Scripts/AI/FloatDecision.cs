using System;

namespace MagmaHeart.AI
{
    public class FloatDecision : DecisionNode
    {
        private readonly float m_minValue;
        private readonly float m_maxValue;
        private readonly Func<float> m_testValue;

        public FloatDecision(float minValue, float maxValue, Func<float> testValue)
        {
            m_minValue = minValue;
            m_maxValue = maxValue;
            m_testValue = testValue;
        }

        public override DecisionTreeNode GetBranch()
        {
            float value = m_testValue();

            if (m_maxValue >= value && value >= m_minValue)
                return TrueNode;

            return FalseNode;
        }
    }
}
