using MagmaHeart.AI.Reasoning;

namespace MagmaHeart.Core.Entities.Properties
{
    public record HealthProperty(float CurrentHealth, float MaxHealth) : PropertySnapshot;
}
