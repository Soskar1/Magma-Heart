namespace MagmaHeart.Core.Spawning
{
    public class MagmaHeartSpawner
    {
        public EnemySpawner EnemySpawner { get; init; }

        public MagmaHeartSpawner(EnemySpawner enemySpawner)
        {
            EnemySpawner = enemySpawner;
        }
    }
}