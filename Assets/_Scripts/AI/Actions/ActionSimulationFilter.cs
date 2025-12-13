using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulationFilter
    {
        public static List<ActionSimulation> GetActionSimulations(SimulatedBoardState simulation, AIUnitModel executor, IArgumentResolver argumentResolver)
        {
            List<ActionSimulation> actionSimulations = new List<ActionSimulation>();
            foreach (UnitAction action in executor.PossibleActions)
            {
                ActionSimulation actionSimulation = new ActionSimulation(action);
                IEnumerable<ActionArgs> resolvedArguments = argumentResolver.Resolve(action, executor, simulation);

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
