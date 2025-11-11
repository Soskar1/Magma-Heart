using MagmaHeart.AI;

namespace MagmaHeart.Core.Entities.Properties
{
    public record HealthPropertySnapshot(float CurrentHealth, float MaxHealth) : PropertySnapshot;
}
