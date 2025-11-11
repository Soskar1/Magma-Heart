using MagmaHeart.AI.Actions;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public record MoveActionArgs(Vector2 Target) : ActionArgs;
}