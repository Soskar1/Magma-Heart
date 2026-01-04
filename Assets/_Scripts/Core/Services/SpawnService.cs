using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.Services
{
    public class SpawnService
    {
        public EntitySpawner EntitySpawner { get; init; }
        public ProjectileSpawner ProjectileSpawner { get; init; }

        public SpawnService(EntitySpawner entitySpawner, ProjectileSpawner projectileSpawner)
        {
            EntitySpawner = entitySpawner;
            ProjectileSpawner = projectileSpawner;
        }
    }
}