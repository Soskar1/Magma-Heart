using System.Collections.Generic;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Spawning;
using MagmaHeart.DungeonGeneration;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleInitializer
    {
        private readonly EntitySpawner m_entitySpawner;
        private readonly AIEngine m_aiEngine;
        private readonly RoomGrid m_grid;
        private readonly Random m_random;
        private readonly float m_minDistanceFromPlayer;

        public BattleInitializer(EntitySpawner entitySpawner, AIEngine aiEngine, Random random, RoomGrid grid, float minDistanceFromPlayer)
        {
            m_entitySpawner = entitySpawner;
            m_aiEngine = aiEngine;
            m_random = random;
            m_minDistanceFromPlayer = minDistanceFromPlayer;
            m_grid = grid;
        }

        public IEnumerable<Entity> InitializeBattle(Room room, Entity player, List<EntityData> enemyPool)
        {
            List<Entity> entities = new List<Entity>() { player };
            for (int i = 0; i < 2; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                EntityData data = enemyPool[m_random.Next(enemyPool.Count)];
                EnemyTurnController turnController = new EnemyTurnController(m_aiEngine);
                Entity entity = m_entitySpawner.Spawn(data, false, turnController);
                entities.Add(entity);

                DungeonTile dungeonTile = null;
                do
                {
                    dungeonTile = room.RoomModel.GetTileAtIndex(m_random.Next(room.RoomModel.TileCount));
                } while (dungeonTile.Type == TileType.Wall || Vector2.Distance(player.transform.position, dungeonTile.Position) < m_minDistanceFromPlayer);

                entity.transform.position = m_grid.ToTileCenter(dungeonTile.Position);
            }

            return entities;
        }
    }
}