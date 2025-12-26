using MagmaHeart.AI.Actions;

namespace MagmaHeart.AI.Reasoning.Tests
{
    internal record RunAwayActionArgs(AIUnitModel Executor, AIUnitModel RunAwayFrom, RunAwayActionData RunAwayActionData) : ActionArgs(Executor, RunAwayActionData);
}
