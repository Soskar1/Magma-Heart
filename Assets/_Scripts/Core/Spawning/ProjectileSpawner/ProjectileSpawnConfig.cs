using System;
using MagmaHeart.Core.Entities;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class ProjectileSpawnConfig : SpawnConfig
    {
        public ProjectileSpawnConfig(GameObject prefab) : base(prefab) { }

        public override void Initialize(GameObject instance, SpawnContext spawnContext)
        {
            ProjectileSpawnContext projectileSpawnContext = spawnContext as ProjectileSpawnContext;
            if (projectileSpawnContext == null)
                throw new ArgumentException("Invalid context type for ProjectileSpawnContext");

            Projectile projectile = instance.GetComponent<Projectile>();
            projectile.Initialize(projectileSpawnContext.BoardState, projectileSpawnContext.Attacker, projectileSpawnContext.Target, projectileSpawnContext.Damage);
        }
    }
}