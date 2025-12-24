using MagmaHeart.Core.BoardStateSystem;
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

        public Projectile Spawn(Vector2 position, EntityModel attacker, EntityModel target, float damage, CombatBoardState boardState)
        {
            ProjectileSpawnContext context = new ProjectileSpawnContext(position, attacker, target, damage, boardState);
            return Spawn(context).GetComponent<Projectile>();
        }
    }
}