namespace MagmaHeart.Core.Spawning
{
    public class MagmaHeartSpawner
    {
        public EntitySpawner EntitySpawner { get; init; }
        public ProjectileSpawner ProjectileSpawner { get; init; }

        public MagmaHeartSpawner(EntitySpawner entitySpawner, ProjectileSpawner projectileSpawner)
        {
            EntitySpawner = entitySpawner;
            ProjectileSpawner = projectileSpawner;
        }
    }
}