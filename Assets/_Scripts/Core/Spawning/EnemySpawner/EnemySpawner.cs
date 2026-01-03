using MagmaHeart.Core.Entities;
using MagmaHeart.DungeonGeneration;
using MagmaHeart.Spawning;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EnemySpawner : BaseSpawner<EnemySpawnContext>
    {
        private readonly List<GameObject> m_enemyPrefabs;
        private readonly float m_minDistanceFromPlayer;
        private readonly EnemySpawnContextFactory m_enemySpawnContextFactory;

        public EnemySpawner(SpawnService spawnService, float minDistanceFromPlayer, List<GameObject> enemyPrefabs, EnemySpawnContextFactory factory) : base(spawnService)
        {
            m_minDistanceFromPlayer = minDistanceFromPlayer;
            m_enemyPrefabs = enemyPrefabs;
            m_enemySpawnContextFactory = factory;
        }

        public override GameObject Spawn(EnemySpawnContext context)
        {
            GameObject prefabToSpawn = m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Count)];
            return SpawnService.Spawn(prefabToSpawn, context);
        }

        public Entity SpawnInRoomTile(RoomModel roomTileData, Vector2 playerPosition)
        {
            DungeonTile dungeonTile = null;
            do
            {
                dungeonTile = roomTileData.GetTileAtIndex(Random.Range(0, roomTileData.TileCount - 1));
            } while (dungeonTile.Type == TileType.Wall || Vector2.Distance(playerPosition, dungeonTile.Position) < m_minDistanceFromPlayer);

            EnemySpawnContext context = (EnemySpawnContext)m_enemySpawnContextFactory.CreateSpawnContext(dungeonTile.Position);
            return Spawn(context).GetComponent<Entity>();
        }
    }
}