using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.Services
{
    public class SpawnService
    {
        public EntitySpawner EntitySpawner { get; init; }

        public SpawnService(EntitySpawner entitySpawner)
        {
            EntitySpawner = entitySpawner;
        }
    }
}