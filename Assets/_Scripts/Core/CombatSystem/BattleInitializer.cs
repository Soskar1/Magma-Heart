using System;
using System.Collections.Generic;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Spawning;
using MagmaHeart.DungeonGeneration;
using UnityEngine;
using Random = System.Random;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleInitializer : IDisposable
    {
        private readonly EntitySpawner m_entitySpawner;
        private readonly AIEngine m_aiEngine;
        private readonly ActionExecutor m_actionRunner;
        private readonly RoomGrid m_grid;
        private readonly Random m_random;
        private readonly float m_minDistanceFromPlayer;

        // Note: We only need the OnLocationChanged event to adjust difficulty.
        // Difficulty is only stat based. In the future, difficulty handling will be different and this implementation will be removed
        private readonly DungeonController m_dungeonController;
        private int m_additionalMonsters = 0;
        private int m_additionalHealth = 0;
        private int m_additionalStrength = 0;
        private int m_additionalSpeed = 0;
        private int m_additionalEnergy = 0;
        private int m_additionalEnergyRegeneration = 0;

        public BattleInitializer(EntitySpawner entitySpawner, AIEngine aiEngine, Random random, RoomGrid grid, float minDistanceFromPlayer, DungeonController dungeonController, ActionExecutor actionRunner)
        {
            m_entitySpawner = entitySpawner;
            m_aiEngine = aiEngine;
            m_random = random;
            m_minDistanceFromPlayer = minDistanceFromPlayer;
            m_grid = grid;
            m_actionRunner = actionRunner;

            m_dungeonController = dungeonController;
            m_dungeonController.OnLocationChanged += HandleOnLocationChanged;
        }

        public void Dispose() => m_dungeonController.OnLocationChanged -= HandleOnLocationChanged;

        public IEnumerable<Entity> InitializeBattle(Room room, Entity player)
        {
            List<EntityData> enemyPool = room.RoomData.EnemyPool;
            List<Entity> entities = new List<Entity>() { player };
            List<Vector2Int> occupiedTiles = new List<Vector2Int>();

            for (int i = 0; i < room.RoomData.EnemyCount + m_additionalMonsters; ++i)
            {
                EntityData data = enemyPool[m_random.Next(enemyPool.Count)];
                EnemyTurnController turnController = new EnemyTurnController(m_aiEngine, m_actionRunner);
                Entity entity = m_entitySpawner.Spawn(data, false, turnController);

                // A dummy way to increase difficulty. In the future, difficulty handling will be different and this implementation will be removed
                entity.Model.Speed.CurrentSpeed = entity.Model.Speed.CurrentSpeed + m_additionalSpeed;
                entity.Model.Strength.CurrentStrength = entity.Model.Strength.CurrentStrength + m_additionalStrength;
                entity.Model.Health.MaxHealth = entity.Model.Health.MaxHealth + m_additionalHealth;
                entity.Model.Health.CurrentHealth = entity.Model.Health.MaxHealth;
                entity.Model.Energy.MaxEnergy = entity.Model.Energy.MaxEnergy + m_additionalEnergy;
                entity.Model.Energy.EnergyRegenerationPerTurn = entity.Model.Energy.EnergyRegenerationPerTurn + m_additionalEnergyRegeneration;

                entities.Add(entity);

                DungeonTile dungeonTile = null;
                do
                {
                    dungeonTile = room.RoomModel.GetTileAtIndex(m_random.Next(room.RoomModel.TileCount));
                } while (dungeonTile.Type == TileType.Wall || Vector2.Distance(player.transform.position, dungeonTile.Position) < m_minDistanceFromPlayer || occupiedTiles.Contains(dungeonTile.Position));

                entity.transform.position = m_grid.ToTileCenter(dungeonTile.Position);
                entity.Model.TilePosition = m_grid.WorldToTilePosition(entity.transform.position);
                occupiedTiles.Add(dungeonTile.Position);
            }

            return entities;
        }

        private void HandleOnLocationChanged(object sender, EventArgs e)
        {
            // Note: a dummy way to increase difficulty.
            // TODO: change this or remove this in the future

            double decision = m_random.NextDouble();

            // Monster count increase or state increase
            if (decision < 0.5f)
            {
                ++m_additionalMonsters;
                return;
            }

            // Stat increase: health or damage or speed
            decision = m_random.NextDouble();

            if (decision < 0.25f)
            {
                m_additionalHealth += 2;
            }
            else if (decision < 0.5f)
            {
                ++m_additionalStrength;
            }
            else if (decision < 0.75f)
            {
                ++m_additionalSpeed;
            } else
            {
                ++m_additionalEnergy;
                ++m_additionalEnergyRegeneration;
            }
        }
    }
}