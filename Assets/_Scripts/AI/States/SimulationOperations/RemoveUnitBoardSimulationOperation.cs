using UnityEngine;

namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record RemoveUnitBoardSimulationOperation(Vector2 Position, AIUnit RemovedUnit) : BoardSimulationOperation(Position);
}
