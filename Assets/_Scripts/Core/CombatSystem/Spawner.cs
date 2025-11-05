using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Spawner
    {
        private readonly DungeonGrid m_grid;
        private readonly Enemy m_enemyPrefab;
        private readonly Player m_player;
        private readonly float m_minDistanceFromPlayer;

        // TODO: Do something with this
        private readonly Vector3 m_offset = new Vector2(0.5f, 0.5f); // Used for offsetting spawned enemies

        public Spawner(Player player, Enemy enemyPrefab, float minDistanceFromPlayer, DungeonGrid grid)
        {
            m_player = player;
            m_enemyPrefab = enemyPrefab;
            m_minDistanceFromPlayer = minDistanceFromPlayer;
            m_grid = grid;
        }

        public Enemy SpawnEnemy(RoomTileData roomTileData)
        {
            DungeonTile dungeonTile = null;
            do
            {
                dungeonTile = roomTileData.GetTileAtIndex(Random.Range(0, roomTileData.TileCount - 1));
            } while (dungeonTile.Type == TileType.Wall || Vector2.Distance(m_player.transform.position, dungeonTile.Position) < m_minDistanceFromPlayer);

            // TODO: Use object pooling
            Enemy entityInstance = Object.Instantiate(m_enemyPrefab, dungeonTile.Position.ToVector3() + m_offset, Quaternion.identity);
            entityInstance.Initialize(m_grid);

            return entityInstance;
        }
    }
}