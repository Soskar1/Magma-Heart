using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record MovementActionArgs(EntityModel TypedExecutor, Vector2 SourceTile, Vector2 TileToMove, MovementActionPayload Payload) : ActionArgs<EntityModel>(TypedExecutor);
}
