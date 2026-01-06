using MagmaHeart.AI.Actions;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record TargetPositionActionInput(AIUnitModel Executor, Vector2 Target) : ActionInput(Executor);
}
