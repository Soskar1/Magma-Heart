using MagmaHeart.Collections;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private readonly Entity m_player;
        private readonly Spawner m_spawner;
        private readonly CircularList<Entity> m_turnOrder;

        private Room m_currentRoom;
        private List<Entity> m_currentEntitiesInBattle;
        
        public event EventHandler OnPlayerVictory;

        private bool m_battleEnded = false;

        public Battle(Entity player, Spawner spawner)
        {
            m_player = player;
            m_spawner = spawner;

            m_turnOrder = new CircularList<Entity>();
        }

        public async Task Start(Room room)
        {
            m_currentRoom = room;
            m_currentEntitiesInBattle = new List<Entity>() { m_player };

            for (int i = 0; i < 1; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                Enemy spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                m_currentEntitiesInBattle.Add(spawnedEntity);
            }

            IEnumerable<Entity> sortedEntities = IniciativeRollSort.SortByRollingIniciative(m_currentEntitiesInBattle);

            foreach (Entity entity in sortedEntities)
            {
                entity.Health.OnDeath += HandleEntityDeath;
                m_currentRoom.AddEntityToInspect(entity);
            }

            m_turnOrder.Clear();
            m_turnOrder.AddRange(m_currentEntitiesInBattle);

            foreach (Entity entity in m_currentEntitiesInBattle)
                entity.CombatController.StartBattle(room);

            await ProcessBattle();
        }

        private async Task ProcessBattle()
        {
            while (!m_battleEnded)
            {
                Entity entity = m_turnOrder.Head;

                Debug.Log($"{entity.gameObject.name} started it's turn");
                await entity.CombatController.StartTurn();
                Debug.Log($"{entity.gameObject.name} ended it's turn");

                m_turnOrder.Next();
            }
        }

        private void HandleEntityDeath(object obj, EventArgs args)
        {
            Health health = obj as Health;
            health.OnDeath -= HandleEntityDeath;

            Entity entity = null;
            foreach (Entity e in m_currentEntitiesInBattle)
            {
                if (e.Health == health)
                {
                    entity = e;
                    break;
                }
            }

            if (entity.Model.IsPlayer)
            {
                End(isPlayerVictory: false);
            }
            else
            {
                m_turnOrder.Remove(entity);
                m_currentEntitiesInBattle.Remove(entity);
                m_currentRoom.RemoveEntityFromRoom(entity);

                bool anyEnemiesInRoom = m_currentEntitiesInBattle.Any(e => e.Model.IsPlayer == false);
                if (!anyEnemiesInRoom)
                {
                    // Win
                    End(isPlayerVictory: true);
                }
            }

            Debug.Log($"Entity died: {entity.gameObject.name}");
            GameObject.Destroy(entity.gameObject); // TODO: Use object pool instead of destroying
        }

        private void End(bool isPlayerVictory)
        {
            if (isPlayerVictory)
            {
                m_turnOrder.Clear();
                m_currentEntitiesInBattle.Clear();

                Debug.Log("Player won the battle!");
                OnPlayerVictory?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // TODO: Handle player defeat
            }

            m_battleEnded = true;
        }
    }
}