using MagmaHeart.AI.Actions;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record MoveActionArgs(AIUnitModel Executor, Vector2 Target, float Speed) : ActionArgs(Executor);
}