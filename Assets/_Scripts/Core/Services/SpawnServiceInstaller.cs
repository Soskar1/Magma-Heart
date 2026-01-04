using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.Services
{
    public class SpawnServiceInstaller : IInstaller
    {
        public SpawnService Install(Entity entityPrefab, Projectile projectilePrefab, RoomGrid roomGrid)
        {
            EntitySpawner enemySpawner = new EntitySpawner(entityPrefab, roomGrid);
            ProjectileSpawner projectileSpawner = new ProjectileSpawner(projectilePrefab);
            
            return new SpawnService(enemySpawner, projectileSpawner);
        }

        public void Dispose() { }
    }
}
