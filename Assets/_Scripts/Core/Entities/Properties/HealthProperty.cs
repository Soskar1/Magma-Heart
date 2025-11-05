using MagmaHeart.AI;

namespace MagmaHeart.Core.Entities.Properties
{
    public record HealthProperty(float CurrentHealth, float MaxHealth) : PropertySnapshot;
}
