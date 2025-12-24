using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Spawning;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.Spawning
{
    public class EnemySpawner : BaseSpawner<EnemySpawnContext>
    {
        private readonly List<GameObject> m_enemyPrefabs;
        private readonly Player m_player;
        private readonly float m_minDistanceFromPlayer;
        private readonly SpawnService m_spawnService;
        private readonly EnemySpawnContextFactory m_enemySpawnContextFactory;

        public EnemySpawner(SpawnService spawnService, Player player, float minDistanceFromPlayer, List<GameObject> enemyPrefabs, EnemySpawnContextFactory factory) : base(spawnService)
        {
            m_spawnService = spawnService;
            m_player = player;
            m_minDistanceFromPlayer = minDistanceFromPlayer;
            m_enemyPrefabs = enemyPrefabs;
            m_enemySpawnContextFactory = factory;
        }

        public override GameObject Spawn(Vector2 position)
        {
            GameObject prefabToSpawn = m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Count)];
            SpawnContext context = m_enemySpawnContextFactory.CreateSpawnContext(position);

            return m_spawnService.Spawn(prefabToSpawn, context);
        }

        public Enemy SpawnInRoomTile(RoomTileData roomTileData)
        {
            DungeonTile dungeonTile = null;
            do
            {
                dungeonTile = roomTileData.GetTileAtIndex(Random.Range(0, roomTileData.TileCount - 1));
            } while (dungeonTile.Type == TileType.Wall || Vector2.Distance(m_player.transform.position, dungeonTile.Position) < m_minDistanceFromPlayer);

            return Spawn(dungeonTile.Position).GetComponent<Enemy>();
        }
    }
}