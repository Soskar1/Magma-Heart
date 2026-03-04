using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.Services
{
    public class ServiceInstaller : IInstaller
    {
        public MagmaHeartServices Install(SpawnService spawnService)
        {
            EntityMovementService movementService = new EntityMovementService();

            return new MagmaHeartServices(movementService, spawnService);
        }

        public void Dispose() { }
    }
}