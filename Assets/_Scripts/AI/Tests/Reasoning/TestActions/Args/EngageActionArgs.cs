using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record EngageActionArgs(AIUnitModel Executor, AIUnitModel Target, float Damage, float Speed) : ActionArgs(Executor);
}