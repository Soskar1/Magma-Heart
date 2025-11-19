using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulationFilter
    {
        public static List<ActionSimulation> GetActionSimulations(SimulatedBoardState simulation, IEnumerable<UnitAction> possibleActions)
        {
            List<ActionSimulation> actionSimulations = new List<ActionSimulation>();
            foreach (UnitAction action in possibleActions)
            {
                ActionSimulation actionSimulation = new ActionSimulation(action);
                List<ActionArgs> possibleSimulations = action.GetArguments(simulation);

                foreach (ActionArgs args in possibleSimulations)
                    if (action.CanExecute(args, simulation))
                        actionSimulation.SimulationArgs.Add(args);

                if (actionSimulation.SimulationArgs.Count > 0)
                    actionSimulations.Add(actionSimulation);
            }

            return actionSimulations;
        }
    }
}
