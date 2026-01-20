using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record RunAwayActionArgs(TargetEntityActionInput TypedInput, RunAwayActionData RunAwayActionData) : ActionArgs(TypedInput);
}
