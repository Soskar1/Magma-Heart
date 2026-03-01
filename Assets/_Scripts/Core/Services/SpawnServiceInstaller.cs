using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.Services
{
    public class SpawnServiceInstaller : IInstaller
    {
        public SpawnService Install(Entity entityPrefab, Projectile projectilePrefab, WorldGrid worldGird)
        {
            EntitySpawner enemySpawner = new EntitySpawner(entityPrefab, worldGird);
            
            return new SpawnService(enemySpawner);
        }

        public void Dispose() { }
    }
}
