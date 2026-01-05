using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts.StatModifiers
{
    public class HealthStatModifier : IStatModifier
    {
        public float AdditionalHealth { get; init; }

        public HealthStatModifier(float additionalHealth) => AdditionalHealth = additionalHealth;

        public void Apply(EntityModel entity)
        {
            entity.Health.MaxHealth += AdditionalHealth;
            entity.Health.CurrentHealth += AdditionalHealth;
        }

        public void Revert(EntityModel entity) => entity.Health.MaxHealth -= AdditionalHealth;
    }
}
