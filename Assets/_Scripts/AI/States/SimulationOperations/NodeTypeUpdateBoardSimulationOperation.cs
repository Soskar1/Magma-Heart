using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record NodeTypeUpdateBoardSimulationOperation(Vector2 Position, BoardNodeType OldNodeType, BoardNodeType NewNodeType) : BoardSimulationOperation(Position);
}
