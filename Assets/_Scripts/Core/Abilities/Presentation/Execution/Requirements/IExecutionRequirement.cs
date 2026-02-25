using MagmaHeart.Abilities;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Requirements
{
    public interface IExecutionRequirement
    {
        bool IsMet(AbilityPlan plan);
    }
}
