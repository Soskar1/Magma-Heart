using MagmaHeart.Core.Entities;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class ProjectileSpawnContext : SpawnContext
    {
        public EntityModel Attacker { get; init; }

        public ProjectileSpawnContext(Vector2 position, EntityModel attacker) : base(position)
        {
            Attacker = attacker;
        }
    }
}