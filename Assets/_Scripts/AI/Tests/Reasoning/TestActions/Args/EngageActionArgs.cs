using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record EngageActionArgs(AIUnitModel Executor, AIUnitModel Target, EngageActionData EngageActionData) : ActionArgs(Executor, EngageActionData);
}