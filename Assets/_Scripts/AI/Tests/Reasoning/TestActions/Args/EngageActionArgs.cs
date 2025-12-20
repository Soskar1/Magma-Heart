using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    public record EngageActionArgs(AIUnitModel Executor, AIUnitModel Target, float Damage, float Speed) : ActionArgs(Executor);
}