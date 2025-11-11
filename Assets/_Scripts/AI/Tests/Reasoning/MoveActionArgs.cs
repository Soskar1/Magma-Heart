using UnityEngine;

namespace MagmaHeart.AI
{
    public record MoveActionArgs(Vector2 Target) : ActionArgs;
}