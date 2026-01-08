using MagmaHeart.Core.CombatSystem;

namespace MagmaHeart.Core.Services
{
    public record MagmaHeartServices(
        EntityMovementService MovementService,
        EntityAttackService AttackService,
        SpawnService SpawnService);
}