using UnityEngine;

namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record AddUnitBoardSimulationOperation(Vector2 Position, AIUnitModel AddedUnit) : BoardSimulationOperation(Position);
}
