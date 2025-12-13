using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public record AttackActionArgs(AIUnitModel Executor, AIUnitModel Target) : ActionArgs(Executor);
}