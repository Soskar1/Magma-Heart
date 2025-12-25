using MagmaHeart.Core.Entities;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class ProjectileSpawner : BaseSpawner<ProjectileSpawnContext>
    {
        private readonly GameObject m_projectilePrefab;

        public ProjectileSpawner(SpawnService spawnService, GameObject projectilePrefab) : base(spawnService)
        {
            m_projectilePrefab = projectilePrefab;
        }

        public override GameObject Spawn(ProjectileSpawnContext context)
        {
            return SpawnService.Spawn(m_projectilePrefab, context);
        }

        public Projectile Spawn(Vector2 position, EntityModel attacker)
        {
            ProjectileSpawnContext context = new ProjectileSpawnContext(position, attacker);
            return Spawn(context).GetComponent<Projectile>();
        }
    }
}