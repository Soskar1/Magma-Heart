using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private readonly TurnBasedPlayerBehaviour m_player;
        private readonly List<ICombatTurnSwitchListener> m_turnSwitchListeners;
        private readonly TurnSwitcher m_turnSwitcher;
        private readonly Spawner m_spawner;

        private Room m_currentRoom;
        private List<ICombatController> m_currentEntitiesInBattle;
        
        public EventHandler OnPlayerVictory;

        public Battle(TurnBasedPlayerBehaviour player, Spawner spawner, List<ICombatTurnSwitchListener> turnSwitchListeners)
        {
            m_player = player;
            m_spawner = spawner;

            m_turnSwitchListeners = turnSwitchListeners;
            m_turnSwitcher = new TurnSwitcher();
        }

        public void Start(Room room)
        {
            m_currentRoom = room;
            m_currentEntitiesInBattle = new List<ICombatController>() { m_player };

            for (int i = 0; i < 3; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                ICombatController spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                m_currentEntitiesInBattle.Add(spawnedEntity);
            }

            IEnumerable<ICombatController> sortedEntities = IniciativeRollSort.SortByRollingIniciative(m_currentEntitiesInBattle);

            foreach (ICombatController controller in sortedEntities)
            {
                controller.StartCombat(m_currentRoom);
                controller.Health.OnDeath += HandleEntityDeath;

                m_currentRoom.AddEntityToInspect(controller);
            }

            foreach (ICombatTurnSwitchListener listener in m_turnSwitchListeners)
                m_turnSwitcher.OnTurnSwitched += listener.HandleOnTurnSwitched;

            m_turnSwitcher.Start(m_currentEntitiesInBattle);
        }

        private void HandleEntityDeath(object obj, EventArgs args)
        {
            Health health = obj as Health;
            health.OnDeath -= HandleEntityDeath;

            ICombatController controller = null;
            foreach (ICombatController entity in m_currentEntitiesInBattle)
            {
                if (entity.Health == health)
                {
                    controller = entity;
                    break;
                }
            }

            if (controller.IsPlayableCharacter)
            {
                End(isPlayerVictory: false);
            }
            else
            {
                m_turnSwitcher.TurnOrder.Remove(controller);
                m_currentEntitiesInBattle.Remove(controller);
                m_currentRoom.RemoveEntityFromRoom(controller);

                bool anyEnemiesInRoom = m_currentEntitiesInBattle.Any(e => e.IsPlayableCharacter == false);
                if (!anyEnemiesInRoom)
                {
                    // Win
                    End(isPlayerVictory: true);
                }
            }

            Debug.Log($"Entity died: {controller.Transform.gameObject.name}");
            GameObject.Destroy(controller.Transform.gameObject); // TODO: Use object pool instead of destroying
        }

        private void End(bool isPlayerVictory)
        {
            foreach (ICombatTurnSwitchListener listener in m_turnSwitchListeners)
                m_turnSwitcher.OnTurnSwitched -= listener.HandleOnTurnSwitched;

            if (isPlayerVictory)
            {
                m_currentEntitiesInBattle.ForEach(survivor =>
                {
                    survivor.NextTurn = null;
                    survivor.EndTurn();
                });

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