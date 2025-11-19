using System.Collections.Generic;

namespace MagmaHeart.AI.Actions
{
    internal class ActionSimulation
    {
        public UnitAction Action { get; init; }
        public List<ActionArgs> SimulationArgs { get; init; }

        public ActionSimulation(UnitAction action)
        {
            Action = action;
            SimulationArgs = new List<ActionArgs>();
        }
    }
}
