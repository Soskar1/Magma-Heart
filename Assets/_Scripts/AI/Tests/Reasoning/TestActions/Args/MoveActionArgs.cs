using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record MoveActionArgs(TargetPositionActionInput TypedInput, MoveActionData MoveActionData) : ActionArgs(TypedInput);
}