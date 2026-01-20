using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Input
{
    public record TargetPositionActionInput(EntityModel TypedExecutor, Vector2 Target) : MagmaHeartActionInput(TypedExecutor);
}
