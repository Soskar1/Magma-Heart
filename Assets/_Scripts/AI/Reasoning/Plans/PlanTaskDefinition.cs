using System;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public record PlanTaskDefinition(Type ActionType, bool ExecuteUntilFail = false);
}
