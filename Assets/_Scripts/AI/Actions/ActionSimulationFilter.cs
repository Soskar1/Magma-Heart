using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulationFilter
    {
        private readonly ActionDatabase m_database;

        public ActionSimulationFilter(ActionDatabase database)
        {
            m_database = database;
        }

        public List<ActionSimulation> GetActionSimulations(SimulatedBoardState simulation, AIUnitModel executor)
        {
            List<ActionSimulation> actionSimulations = new List<ActionSimulation>();

            foreach (ActionDefinition actionDefinition in executor.PossibleActions)
            {
                UnitAction action = m_database.Get(actionDefinition.ActionType);

                ActionSimulation actionSimulation = new ActionSimulation(action);

                foreach (AIUnitModel target in actionDefinition.TargetSelector.SelectTargets(simulation, executor))
                {
                    IEnumerable<ActionArgs> generatedArguments = actionDefinition.CreateArguments(executor, target, simulation);

                    foreach (ActionArgs arguments in generatedArguments)
                        if (action.CanExecute(arguments, simulation))
                            actionSimulation.SimulationArgs.Add(arguments);
                }

                if (actionSimulation.SimulationArgs.Count > 0)
                    actionSimulations.Add(actionSimulation);
            }

            return actionSimulations;
        }
    }
}
