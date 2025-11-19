using MagmaHeart.AI.Boards;
using UnityEngine;

namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record NodeTypeReplaceBoardSimulationOperation(Vector2 Position, BoardNodeType From, BoardNodeType To) : BoardSimulationOperation(Position);
}
