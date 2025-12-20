using MagmaHeart.AI.Actions;
using System.Collections.Generic;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public record PlanDefinition(List<PlanTaskDefinition> TaskDefinitions, IActionTargetSelector TargetSelector);
}
