using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulationFilter
    {
        private readonly IArgumentResolver m_argumentResolver;
        private readonly ActionDatabase m_database;

        public ActionSimulationFilter(IArgumentResolver argumentResolver, ActionDatabase database)
        {
            m_argumentResolver = argumentResolver;
            m_database = database;
        }

        public List<ActionSimulation> GetActionSimulations(SimulatedBoardState simulation, AIUnitModel executor)
        {
            List<ActionSimulation> actionSimulations = new List<ActionSimulation>();

            foreach (ActionEntry entry in executor.PossibleActions)
            {
                UnitAction action = m_database.Get(entry.ActionType);

                ActionSimulation actionSimulation = new ActionSimulation(action);
                IEnumerable<ActionArgs> resolvedArguments = m_argumentResolver.Resolve(action, executor, entry.Payload, simulation);

                foreach (ActionArgs arguments in resolvedArguments)
                    if (action.CanExecute(arguments, simulation))
                        actionSimulation.SimulationArgs.Add(arguments);

                if (actionSimulation.SimulationArgs.Count > 0)
                    actionSimulations.Add(actionSimulation);
            }

            return actionSimulations;
        }
    }
}
