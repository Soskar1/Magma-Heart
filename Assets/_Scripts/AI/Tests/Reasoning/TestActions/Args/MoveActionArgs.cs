using MagmaHeart.AI.Actions;
using UnityEngine;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record MoveActionArgs(AIUnitModel Executor, Vector2 Target, MoveActionData MoveActionData) : ActionArgs(Executor);
}