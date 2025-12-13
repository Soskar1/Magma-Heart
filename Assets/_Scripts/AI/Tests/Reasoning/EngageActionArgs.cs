using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public record EngageActionArgs(AIUnitModel Executor, AIUnitModel Target) : ActionArgs(Executor);
}