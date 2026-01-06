using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record TargetEntityActionInput(AIUnitModel Executor, AIUnitModel Target) : ActionInput(Executor);
}
