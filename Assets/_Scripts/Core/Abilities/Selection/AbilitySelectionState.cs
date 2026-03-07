
using MagmaHeart.Abilities;

namespace MagmaHeart.Core.Abilities.Selection
{
    public sealed class AbilitySelectionState
    {
        public AbilityDefinition ArmedAbility { get; private set; }

        public bool HasArmedAbility => ArmedAbility != null;

        public void Arm(AbilityDefinition ability)
        {
            ArmedAbility = ability;
        }

        public void Disarm()
        {
            ArmedAbility = null;
        }
    }
}
