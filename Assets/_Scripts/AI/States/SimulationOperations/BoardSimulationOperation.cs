using UnityEngine;

namespace MagmaHeart.AI.States.SimulationOperations
{
    internal abstract record BoardSimulationOperation(Vector2 Position) : SimulationOperation;
}
