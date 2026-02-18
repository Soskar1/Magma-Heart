using MagmaHeart.Core.CombatSystem;

namespace MagmaHeart.Core.Services
{
    public record MagmaHeartServices(
        EntityMovementService MovementService,
        SpawnService SpawnService);
}