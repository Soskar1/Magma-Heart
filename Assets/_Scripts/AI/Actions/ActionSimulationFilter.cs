using MagmaHeart.AI.Plans;
using MagmaHeart.AI.Reasoning;
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
                Type actionType = planDefinition.TaskDefinition.ActionType;
                ActionDefinition actionDefinition = executor.PossibleActions.Where(action => action.ActionType == actionType).FirstOrDefault();

                if (actionDefinition is null)
                    continue;

                UnitAction action = m_database.Get(actionDefinition.ActionType);
                PlanTask planTask = new PlanTask(action);
                Plan plan = new Plan(planTask);
                PlanSimulation planSimulation = new PlanSimulation(plan);

                foreach (AIUnitModel target in actionDefinition.TargetSelector.SelectTargets(simulation, executor))
                {
                    IEnumerable<ActionArgs> generatedArguments = actionDefinition.CreateArguments(executor, target, simulation);

                    foreach (ActionArgs arguments in generatedArguments)
                        planSimulation.SimulationArgs.Add(arguments);
                }

                if (planSimulation.SimulationArgs.Count > 0)
                    planSimulations.Add(planSimulation);
            }

            return planSimulations;
        }
    }
}
