using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulationFilter
    {
        public static List<ActionSimulation> GetActionSimulations(StateSnapshot state, SimulatedBoard board, List<Action> possibleActions)
        {
            List<ActionSimulation> actionSimulations = new List<ActionSimulation>();
            foreach (Action action in possibleActions)
            {
                ActionSimulation actionSimulation = new ActionSimulation(action);
                List<ActionArgs> possibleSimulations = action.GetSimulationArguments(state);

                foreach (ActionArgs args in possibleSimulations)
                    if (action.CanSimulate(state, board, args))
                        actionSimulation.SimulationArgs.Add(args);

                if (actionSimulation.SimulationArgs.Count > 0)
                    actionSimulations.Add(actionSimulation);
            }

            return actionSimulations;
        }
    }
}
