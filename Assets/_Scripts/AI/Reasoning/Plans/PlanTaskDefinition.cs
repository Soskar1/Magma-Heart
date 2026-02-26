using MagmaHeart.Abilities;

namespace MagmaHeart.AI.Reasoning.Plans
{
    public record PlanTaskDefinition(AbilityDefinition Ability, bool ExecuteUntilFail = false);
}
