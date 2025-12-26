using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record MovementActionArgs(EntityModel TypedExecutor, Vector2 SourceTile, Vector2 TileToMove, MovementActionData MovementActionData) : ActionArgs<EntityModel>(TypedExecutor, MovementActionData);
}
