using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
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
        private readonly List<ITurnSwitchListener> m_turnSwitchListeners;

        private Room m_currentRoom;
        private Dictionary<EntityModel, Entity> m_currentEntitiesInBattle;
        
        public event EventHandler OnPlayerVictory;
        private event EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;

        private bool m_battleEnded = false;

        public Battle(Entity player, Spawner spawner, List<ITurnSwitchListener> turnSwitchListeners)
        {
            m_player = player;
            m_spawner = spawner;

            m_turnOrder = new CircularList<Entity>();
            m_turnSwitchListeners = turnSwitchListeners;

            Enable();
        }

        public void Enable()
        {
            foreach (ITurnSwitchListener listener in m_turnSwitchListeners)
                OnTurnSwitched += listener.HandleOnTurnSwitched;
        }

        public void Disable()
        {
            foreach (ITurnSwitchListener listener in m_turnSwitchListeners)
                OnTurnSwitched -= listener.HandleOnTurnSwitched;
        }

        public async Task Start(Room room)
        {
            m_battleEnded = false;
            m_currentRoom = room;
            m_currentEntitiesInBattle = new Dictionary<EntityModel, Entity>() { { m_player.Model, m_player } };

            for (int i = 0; i < 5; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                Enemy spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                m_currentEntitiesInBattle.Add(spawnedEntity.Model, spawnedEntity);
            }

            List<Entity> entityList = m_currentEntitiesInBattle.Values.ToList();
            IEnumerable<Entity> sortedEntities = IniciativeRollSort.SortByRollingIniciative(entityList);

            foreach (Entity entity in sortedEntities)
            {
                entity.Health.OnDeath += HandleEntityDeath;
                m_currentRoom.AddEntityToInspect(entity);
            }

            m_turnOrder.Clear();
            m_turnOrder.AddRange(entityList);

            CombatBoardState combatBoardState = new CombatBoardState(m_currentRoom);

            foreach (Entity entity in entityList)
                entity.CombatController.StartBattle(combatBoardState, m_turnOrder);

            await ProcessBattle();
        }

        private async Task ProcessBattle()
        {
            while (!m_battleEnded)
            {
                Entity entity = m_turnOrder.Head;

                Debug.Log($"{entity.gameObject.name} started it's turn");

                OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(entity);
                OnTurnSwitched?.Invoke(this, args);
                await entity.CombatController.StartTurn();

                Debug.Log($"{entity.gameObject.name} ended it's turn");

                m_turnOrder.Next();
            }
        }

        private void HandleEntityDeath(object obj, OnDeathEventArgs args)
        {
            EntityModel model = args.Model;
            Entity entity = m_currentEntitiesInBattle[model];
            model.Health.OnDeath -= HandleEntityDeath;

            if (model.IsPlayer)
            {
                End(isPlayerVictory: false);
            }
            else
            {
                m_turnOrder.Remove(entity);
                m_currentEntitiesInBattle.Remove(model);
                m_currentRoom.RemoveEntityFromRoom(entity);

                bool anyEnemiesInRoom = m_currentEntitiesInBattle.Values.Any(e => e.Model.IsPlayer == false);
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
                Debug.Log("Player won the battle!");
                OnPlayerVictory?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // TODO: Handle player defeat
            }

            foreach (Entity entity in m_currentEntitiesInBattle.Values)
            {
                m_currentRoom.RemoveEntityFromRoom(entity);
                entity.CombatController.EndBattle();
            }

            m_turnOrder.Clear();
            m_currentEntitiesInBattle.Clear();

            m_battleEnded = true;
        }
    }
}