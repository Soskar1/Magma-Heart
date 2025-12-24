using System;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Spawning;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EnemySpawnConfig : SpawnConfig
    {
        public EnemySpawnConfig(GameObject prefab) : base(prefab) { }

        public override void Initialize(GameObject instance, SpawnContext spawnContext)
        {
            EnemySpawnContext enemySpawnContext = spawnContext as EnemySpawnContext;
            if (enemySpawnContext == null)
                throw new ArgumentException("Invalid context type for EnemySpawnContext");

            Enemy enemy = instance.GetComponent<Enemy>();
            enemy.Initialize(enemySpawnContext.Grid, enemySpawnContext.CombatAI);
        }
    }
}