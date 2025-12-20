using MagmaHeart.AI.Actions;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    internal class PlanSimulation
    {
        public Plan Plan { get; init; }
        public List<ActionArgs> SimulationArgs { get; init; }

        public PlanSimulation(Plan plan)
        {
            Plan = plan;
            SimulationArgs = new List<ActionArgs>();
        }
    }
}
