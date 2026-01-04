using MagmaHeart.Core.AI;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.Spawning
{
    public class SpawnerInstaller : IInstaller
    {
        public MagmaHeartSpawner Install(Entity entityPrefab, Projectile projectilePrefab, RoomGrid roomGrid)
        {
            EntitySpawner enemySpawner = new EntitySpawner(entityPrefab, roomGrid);
            ProjectileSpawner projectileSpawner = new ProjectileSpawner(projectilePrefab);
            
            return new MagmaHeartSpawner(enemySpawner, projectileSpawner);
        }

        public void Dispose() { }
    }
}
