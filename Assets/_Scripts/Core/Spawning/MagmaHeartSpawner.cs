namespace MagmaHeart.Core.Spawning
{
    public class MagmaHeartSpawner
    {
        public EnemySpawner EnemySpawner { get; init; }
        public ProjectileSpawner ProjectileSpawner { get; init; }

        public MagmaHeartSpawner(EnemySpawner enemySpawner, ProjectileSpawner projectileSpawner)
        {
            EnemySpawner = enemySpawner;
            ProjectileSpawner = projectileSpawner;
        }
    }
}