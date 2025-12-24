using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class ProjectileSpawnContext : SpawnContext
    {
        public EntityModel Attacker { get; init; }
        public EntityModel Target { get; init; }
        public float Damage { get; init; }
        public CombatBoardState BoardState { get; init; }

        public ProjectileSpawnContext(Vector2 position, EntityModel attacker, EntityModel target, float damage, CombatBoardState boardState) : base(position)
        {
            Attacker = attacker;
            Target = target;
            Damage = damage;
            BoardState = boardState;
        }
    }
}