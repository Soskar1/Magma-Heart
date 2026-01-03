using System;
using MagmaHeart.Core.Entities;
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

            Entity enemy = instance.GetComponent<Entity>();
            EnemyTurnController turnController = new EnemyTurnController(enemySpawnContext.AiEngine);
            enemy.Initialize(enemySpawnContext.Grid, false, turnController);
        }
    }
}