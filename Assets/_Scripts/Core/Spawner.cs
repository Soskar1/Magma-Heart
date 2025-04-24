using System;
using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MagmaHeart.Core
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int m_amountOfEnemies;
        [SerializeField] private float m_minDistanceFromPlayer;
        [SerializeField] private Vector2 m_offset;
        [SerializeField] private int m_initialPoolSize;

        private Room m_currentRoom;
        private Entity m_player;

        private ObjectPoolMap m_enemyObjectPoolMap;
        private List<EnemyMeleeBehaviour> m_spawnedEnemies;

        public Action SpawnedEnemy;
        public Action EnemyDisabled;

        public void Initialize(Entity player)
        {
            m_player = player;

            m_spawnedEnemies = new List<EnemyMeleeBehaviour>();
            m_enemyObjectPoolMap = new ObjectPoolMap();
        }

        public void SetRoom(Room room) => m_currentRoom = room;

        public void SpawnWave()
        {
            RoomTileData tileData = m_currentRoom.roomTileData;

            foreach (RoomEnemy enemy in m_currentRoom.Enemies)
            {
                if (!m_enemyObjectPoolMap.IsRegistered(enemy.prefab.GetType()))
                    m_enemyObjectPoolMap.RegisterPool<EnemyMeleeBehaviour>(enemy.prefab, EnemyInitialization, m_initialPoolSize);

                for (int i = 0; i < enemy.count; ++i)
                {
                    DungeonTile dungeonTile = null;
                    while (dungeonTile == null || dungeonTile.Type == TileType.Wall || Vector2.Distance(m_player.transform.position, dungeonTile.Position) < m_minDistanceFromPlayer)
                        dungeonTile = tileData.GetTileAtIndex(Random.Range(0, tileData.TileCount - 1));

                    EnemyMeleeBehaviour enemyInstance = m_enemyObjectPoolMap.Get<EnemyMeleeBehaviour>(enemy.prefab.GetType());
                    enemyInstance.transform.position = dungeonTile.Position.ToVector2() + m_offset;
                    SpawnedEnemy?.Invoke();

                    foreach (EnemyMeleeBehaviour other in m_spawnedEnemies)
                    {
                        Physics2D.IgnoreCollision(other.Collider, enemyInstance.AttackHitCollider);
                        Physics2D.IgnoreCollision(enemyInstance.Collider, other.AttackHitCollider);
                    }
                }
            }
        }

        private void PushToPool(EnemyMeleeBehaviour enemy)
        {
            m_enemyObjectPoolMap.Return(enemy);
            EnemyDisabled?.Invoke();

            m_spawnedEnemies.Remove(enemy);
        }

        private void EnemyInitialization(EnemyMeleeBehaviour newEnemy)
        {
            newEnemy.Initialize(m_player);
            newEnemy.transform.parent = transform;
            newEnemy.OnDisable += PushToPool;

            m_spawnedEnemies.Add(newEnemy);
        }
    }
}