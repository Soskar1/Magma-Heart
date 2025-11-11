using MagmaHeart.AI.States;

namespace MagmaHeart.Core.Entities.Properties
{
    public record HealthPropertySnapshot(float CurrentHealth, float MaxHealth) : PropertySnapshot;
}
