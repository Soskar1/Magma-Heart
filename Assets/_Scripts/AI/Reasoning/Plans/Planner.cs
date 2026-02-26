using System.Collections.Generic;
using MagmaHeart.AI.Execution;

namespace MagmaHeart.AI.Reasoning.Plans
{
    internal class Planner
    {
        private readonly Strategy m_strategy;

        public Planner(Strategy strategy)
        {
            m_strategy = strategy;
        }

        public List<Plan> GetPlans(AIUnitModel executor)
        {
            List<Plan> plans = new List<Plan>();

            foreach (PlanDefinition planDefinition in m_strategy.Plans)
            {
                Plan plan = new Plan(planDefinition.TaskDefinitions);

                if (plan == null)
                    continue;

                plans.Add(plan);
            }

            return plans;
        }
    }
}
