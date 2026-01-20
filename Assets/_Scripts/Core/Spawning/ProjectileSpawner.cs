using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class ProjectileSpawner
    {
        private readonly Projectile m_projectilePrefab;

        public ProjectileSpawner(Projectile projectilePrefab)
        {
            m_projectilePrefab = projectilePrefab;
        }

        public Projectile Spawn(EntityModel attacker)
        {
            Projectile projectile = GameObject.Instantiate(m_projectilePrefab);
            projectile.Initialize(attacker);
            return projectile;
        }
    }
}