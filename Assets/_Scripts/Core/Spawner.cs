using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private EnemyMeleeBehaviour m_enemyToSpawn;
        [SerializeField] private int m_amountOfEnemies;
        [SerializeField] private float m_minDistanceFromPlayer;
        [SerializeField] private Vector2 m_offset;

        private Room m_currentRoom;
        private Entity m_player;

        private ObjectPool<EnemyMeleeBehaviour> m_enemyPool;
        private List<EnemyMeleeBehaviour> m_spawnedEnemies;

        public void Initialize(Entity player)
        {
            m_player = player;

            m_spawnedEnemies = new List<EnemyMeleeBehaviour>();

            m_enemyPool = new ObjectPool<EnemyMeleeBehaviour>(m_enemyToSpawn, (newEnemy) =>
            {
                newEnemy.Initialize(m_player);

                foreach (EnemyMeleeBehaviour enemy in m_spawnedEnemies)
                {
                    Physics2D.IgnoreCollision(enemy.Collider, newEnemy.AttackHitCollider);
                    Physics2D.IgnoreCollision(newEnemy.Collider, enemy.AttackHitCollider);
                }

                newEnemy.OnDisable += m_enemyPool.PushToPool;
                m_spawnedEnemies.Add(newEnemy);
            });
        }

        public void SetRoomTileData(Room room)
        {
            m_currentRoom = room;
            SpawnWave();
            room.playerEnteredRoom -= SetRoomTileData;
        }

        public void SpawnWave()
        {
            RoomTileData tileData = m_currentRoom.RoomTileData;
            for (int i = 0; i < m_amountOfEnemies; ++i)
            {
                DungeonTile dungeonTile = null;
                while (dungeonTile == null || dungeonTile.Type == TileType.Wall || Vector2.Distance(m_player.transform.position, dungeonTile.Position) < m_minDistanceFromPlayer)
                    dungeonTile = tileData.GetTileAtIndex(Random.Range(0, tileData.TileCount - 1));

                EnemyMeleeBehaviour enemyInstance = m_enemyPool.Get();
                enemyInstance.transform.position = dungeonTile.Position.ToVector2() + m_offset;
            }
        }
    }
}