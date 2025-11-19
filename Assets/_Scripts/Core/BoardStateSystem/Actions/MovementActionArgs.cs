using MagmaHeart.AI.Actions;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record MovementActionArgs(Vector2 SourceTile, Vector2 TileToMove) : ActionArgs;
}
