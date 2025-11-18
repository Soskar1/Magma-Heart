using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulationFilter
    {
        public static List<ActionSimulation> GetActionSimulations(GameState gameState, List<Action> possibleActions)
        {
            List<ActionSimulation> actionSimulations = new List<ActionSimulation>();
            foreach (Action action in possibleActions)
            {
                ActionSimulation actionSimulation = new ActionSimulation(action);

                // TODO: maybe we can use a Strategy to generate specific simulation arguments
                List<ActionArgs> possibleSimulations = action.GetSimulationArguments(state, board);

                foreach (ActionArgs args in possibleSimulations)
                    if (action.CanExecute(args, gameState))
                        actionSimulation.SimulationArgs.Add(args);

                if (actionSimulation.SimulationArgs.Count > 0)
                    actionSimulations.Add(actionSimulation);
            }

            return actionSimulations;
        }
    }
}
