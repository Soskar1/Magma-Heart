using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.Services
{
    public class ServiceInstaller : IInstaller
    {
        public MagmaHeartServices Install(SpawnService spawnService)
        {
            EntityAttackService attackService = new EntityAttackService(spawnService);
            EntityMovementService movementService = new EntityMovementService();

            return new MagmaHeartServices(movementService, attackService, spawnService);
        }

        public void Dispose() { }
    }
}