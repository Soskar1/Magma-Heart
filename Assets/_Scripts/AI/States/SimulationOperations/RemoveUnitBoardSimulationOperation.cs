using UnityEngine;

namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record RemoveUnitBoardSimulationOperation(Vector2 Position, AIUnitModel RemovedUnit) : BoardSimulationOperation(Position);
}
