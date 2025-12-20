using MagmaHeart.AI.Actions;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    internal class PlanSimulation
    {
        public Plan Plan { get; init; }
        public IEnumerable<AIUnitModel> Targets { get; init; }

        public PlanSimulation(Plan plan, IEnumerable<AIUnitModel> targets)
        {
            Plan = plan;
            Targets = targets;
        }
    }
}
