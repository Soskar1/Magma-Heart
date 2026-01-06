using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record AttackActionArgs(TargetEntityActionInput TypedInput, AttackActionData AttackActionData) : ActionArgs(TypedInput);
}