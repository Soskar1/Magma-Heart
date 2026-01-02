using MagmaHeart.Core.AI;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Spawning;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class SpawnerInstaller : IInstaller
    {
        public MagmaHeartSpawner Install(IEnumerable<GameObject> enemyPrefabs, GameObject projectilePrefab, AIContext aiContext, RoomGrid roomGrid, float minDistanceFromPlayer)
        {
            Dictionary<GameObject, SpawnConfig> configs = new Dictionary<GameObject, SpawnConfig>();
            foreach (GameObject prefab in enemyPrefabs)
            {
                EnemySpawnConfig config = new EnemySpawnConfig(prefab);
                configs.Add(prefab, config);
            }
            ProjectileSpawnConfig projectileConfig = new ProjectileSpawnConfig(projectilePrefab);
            configs.Add(projectilePrefab, projectileConfig);

            SpawnService spawnService = new SpawnService(configs, new UnityInstantiator());
            EnemySpawnContextFactory factory = new EnemySpawnContextFactory(roomGrid, aiContext.CombatAI);

            EnemySpawner enemySpawner = new EnemySpawner(spawnService, minDistanceFromPlayer, enemyPrefabs.ToList(), factory);
            ProjectileSpawner projectileSpawner = new ProjectileSpawner(spawnService, projectilePrefab);
            
            return new MagmaHeartSpawner(enemySpawner, projectileSpawner);
        }

        public void Dispose() { }
    }
}
