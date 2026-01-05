using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts.StatModifiers
{
    public class StrengthStatModifier : IStatModifier
    {
        public int AdditionalStrength { get; init; }

        public StrengthStatModifier(int additionalStrength) => AdditionalStrength = additionalStrength;

        public void Apply(EntityModel entity) => entity.Strength.CurrentStrength += AdditionalStrength;
        public void Revert(EntityModel entity) => entity.Strength.CurrentStrength -= AdditionalStrength;
    }
}
