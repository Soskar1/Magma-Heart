using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts
{
    public class HealthStatModifier : IStatModifier
    {
        public float AdditionalHealth { get; set; }

        public HealthStatModifier() => AdditionalHealth = 0;
        public HealthStatModifier(float additionalHealth) => AdditionalHealth = additionalHealth;

        public void Apply(Entity entity)
        {
            entity.Health.MaxHealth += AdditionalHealth;
            entity.Health.CurrentHealth += AdditionalHealth;
        }

        public void Revert(Entity entity) => entity.Health.MaxHealth -= AdditionalHealth;
    }
}
