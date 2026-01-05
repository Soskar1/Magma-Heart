using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record AttackActionArgs(AIUnitModel Executor, AIUnitModel Target, AttackActionData AttackActionData) : ActionArgs(Executor);
}