using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private readonly Entity m_player;
        private readonly List<ICombatTurnSwitchListener> m_turnSwitchListeners;
        private readonly List<IBattleStartedListener> m_battleStartedListeners;
        private readonly TurnSwitcher m_turnSwitcher;
        private readonly Spawner m_spawner;

        private Room m_currentRoom;
        private List<Entity> m_currentEntitiesInBattle;
        
        public event EventHandler OnPlayerVictory;
        public event EventHandler<OnBattleStartedEventArgs> OnCombatStarted;

        public Battle(Entity player, Spawner spawner, List<ICombatTurnSwitchListener> turnSwitchListeners, List<IBattleStartedListener> battleStartedListeners)
        {
            m_player = player;
            m_spawner = spawner;

            m_turnSwitchListeners = turnSwitchListeners;
            m_turnSwitcher = new TurnSwitcher();

            Enable();
        }

        public void Enable()
        {
            foreach (IBattleStartedListener listener in m_turnSwitchListeners)
                OnCombatStarted += listener.HandleOnBattleStarted;
        }

        public void Disable()
        {
            foreach (IBattleStartedListener listener in m_turnSwitchListeners)
                OnCombatStarted -= listener.HandleOnBattleStarted;
        }

        public void Start(Room room)
        {
            m_currentRoom = room;
            m_currentEntitiesInBattle = new List<Entity>() { m_player };

            for (int i = 0; i < 2; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                Entity spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                m_currentEntitiesInBattle.Add(spawnedEntity);
            }

            IEnumerable<Entity> sortedEntities = IniciativeRollSort.SortByRollingIniciative(m_currentEntitiesInBattle);

            foreach (Entity entity in sortedEntities)
            {
                entity.Health.OnDeath += HandleEntityDeath;
                m_currentRoom.AddEntityToInspect(entity);
            }

            foreach (ICombatTurnSwitchListener listener in m_turnSwitchListeners)
                m_turnSwitcher.OnTurnSwitched += listener.HandleOnTurnSwitched;

            m_turnSwitcher.Start(m_currentEntitiesInBattle);

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(room);
            OnCombatStarted?.Invoke(this, args);
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
                m_turnSwitcher.TurnOrder.Remove(entity);
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
            foreach (ICombatTurnSwitchListener listener in m_turnSwitchListeners)
                m_turnSwitcher.OnTurnSwitched -= listener.HandleOnTurnSwitched;

            if (isPlayerVictory)
            {
                m_turnSwitcher.Clear();
                m_currentEntitiesInBattle.Clear();

                Debug.Log("Player won the battle!");
                OnPlayerVictory?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // TODO: Handle player defeat
            }
        }
    }
}