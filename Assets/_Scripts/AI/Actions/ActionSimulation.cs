using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulation
    {
        public Action Action { get; init; }
        public List<ActionArgs> SimulationArgs { get; init; }

        public ActionSimulation(Action action)
        {
            Action = action;
            SimulationArgs = new List<ActionArgs>();
        }
    }
}
