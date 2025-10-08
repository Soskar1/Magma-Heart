using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Artifacts
{
    public class HealthStatModifier : IStatModifier
    {
        public float AdditionalHealth { get; set; }

        public HealthStatModifier(float additionalHealth) => AdditionalHealth = additionalHealth;

        public void Apply(Entity entity)
        {
            // Implementation logic
        }
    }
}
