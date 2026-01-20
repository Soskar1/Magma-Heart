using MagmaHeart.AI.States.SimulationOperations;
using System.Collections.Generic;

namespace MagmaHeart.AI.States
{
    internal record SimulationChange(List<SimulationOperation> Operations);
}
