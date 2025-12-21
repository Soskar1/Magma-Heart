using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System.Collections.Generic;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Spawner
    {
        private readonly DungeonGrid m_grid;
        private readonly CombatAI m_ai;
        private readonly List<Enemy> m_enemyPrefabs;
        private readonly Player m_player;
        private readonly float m_minDistanceFromPlayer;

        public Spawner(Player player, List<Enemy> enemyPrefabs, float minDistanceFromPlayer, DungeonGrid grid, CombatAI ai)
        {
            m_player = player;
            m_enemyPrefabs = enemyPrefabs;
            m_minDistanceFromPlayer = minDistanceFromPlayer;
            m_grid = grid;
            m_ai = ai;
        }

        public Enemy SpawnEnemy(RoomTileData roomTileData)
        {
            DungeonTile dungeonTile = null;
            do
            {
                dungeonTile = roomTileData.GetTileAtIndex(Random.Range(0, roomTileData.TileCount - 1));
            } while (dungeonTile.Type == TileType.Wall || Vector2.Distance(m_player.transform.position, dungeonTile.Position) < m_minDistanceFromPlayer);

            Enemy prefabToSpawn = m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Count)];

            // TODO: Use object pooling
            Enemy entityInstance = Object.Instantiate(prefabToSpawn, m_grid.ToTileCenter(dungeonTile.Position), Quaternion.identity);
            entityInstance.Initialize(m_grid, m_ai);

            return entityInstance;
        }
    }
}