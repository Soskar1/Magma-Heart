using System;
using System.Collections.Generic;
using System.Linq;
using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Plans
{
    internal class Planner
    {
        private readonly ActionDatabase m_database;
        private readonly Strategy m_strategy;

        public Planner(Strategy strategy, ActionDatabase database)
        {
            m_database = database;
            m_strategy = strategy;
        }

        public List<Plan> GetPlans(AIUnitModel executor)
        {
            List<Plan> plans = new List<Plan>();

            foreach (PlanDefinition planDefinition in m_strategy.Plans)
            {
                Plan plan = TryCreatePlan(planDefinition, executor);

                if (plan == null)
                    continue;

                plans.Add(plan);
            }

            return plans;
        }

        private Plan TryCreatePlan(PlanDefinition planDefinition, AIUnitModel executor)
        {
            List<PlanTask> planTasks = new List<PlanTask>();

            foreach (PlanTaskDefinition taskDefinition in planDefinition.TaskDefinitions)
            {
                Type actionType = taskDefinition.ActionType;
                ActionDefinition actionDefinition = executor.PossibleActions.Where(action => action.ActionType == actionType).FirstOrDefault();

                if (actionDefinition is null)
                    return null;

                UnitAction action = m_database.Get(actionDefinition.ActionType);
                PlanTask planTask = new PlanTask(action, actionDefinition, taskDefinition.ExecuteUntilFail);
                planTasks.Add(planTask);
            }

            return new Plan(planTasks);
        }
    }
}
