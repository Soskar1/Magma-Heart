using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record AttackActionArgs(AIUnitModel Executor, AIUnitModel Target, float Damage) : ActionArgs(Executor);
}