using MagmaHeart.AI.Reasoning;
using MagmaHeart.AI.Reasoning.Plans;
using MagmaHeart.AI.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulationFilter
    {
        private readonly ActionDatabase m_database;
        private readonly Strategy m_strategy;

        public ActionSimulationFilter(Strategy strategy, ActionDatabase database)
        {
            m_database = database;
            m_strategy = strategy;
        }

        public List<PlanSimulation> GetPossiblePlans(SimulatedBoardState simulation, AIUnitModel executor)
        {
            List<PlanSimulation> planSimulations = new List<PlanSimulation>();

            foreach (PlanDefinition planDefinition in m_strategy.Plans)
            {
                Plan plan = TryCreatePlan(planDefinition, executor);

                if (plan == null)
                    continue;

                List<AIUnitModel> targets = planDefinition.TargetSelector.SelectTargets(simulation, executor).ToList();
                PlanSimulation planSimulation = new PlanSimulation(plan, targets);

                if (targets.Count > 0)
                    planSimulations.Add(planSimulation);
            }

            return planSimulations;
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
