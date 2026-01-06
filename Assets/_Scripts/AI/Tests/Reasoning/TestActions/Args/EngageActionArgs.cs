using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record EngageActionArgs(TargetEntityActionInput TypedInput, EngageActionData EngageActionData) : ActionArgs(TypedInput);
}