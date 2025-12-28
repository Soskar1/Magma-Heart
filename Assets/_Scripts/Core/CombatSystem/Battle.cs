using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Spawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private readonly MagmaHeartSpawner m_spawner;
        private readonly Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>> m_healthHandlers = new Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>>();
        private readonly EntityMovementService m_movementService;

        private Room m_currentRoom;
        private TurnOrder m_currentTurnOrder;

        public Room CurrentRoom => m_currentRoom;
        public event EventHandler<OnBattleStartedEventArgs> OnBattleStarted;
        public event EventHandler<OnBattleEndedEventArgs> OnBattleEnded;
        public event EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;
        public event EventHandler<OnEntityDiedEventArgs> OnEntityDied;

        private bool m_battleEnded = false;

        public Battle(MagmaHeartSpawner spawner, EntityMovementService movementService)
        {
            m_spawner = spawner;
            m_movementService = movementService;
        }

        public async Task Start(Room room, Player player)
        {
            m_battleEnded = false;
            m_currentRoom = room;

            m_currentRoom.AddEntityToInspect(player);
            for (int i = 0; i < 2; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                Enemy spawnedEntity = m_spawner.EnemySpawner.SpawnInRoomTile(room.RoomModel, player.transform.position);
                m_currentRoom.AddEntityToInspect(spawnedEntity);

                EventHandler<OnHealthChangedEventArgs> handler = new EventHandler<OnHealthChangedEventArgs>((sender, args) =>
                {
                    HandleEntityOnHealthChanged(spawnedEntity.Model, args);
                });

                m_healthHandlers[spawnedEntity.Model] = handler;
                spawnedEntity.Health.OnHealthChanged += handler;
            }

            IEnumerable<Entity> sortedEntities = IniciativeRollSort.SortByRollingIniciative(m_currentRoom.Entities);

            m_currentTurnOrder = new TurnOrder(sortedEntities.Select(e => e.TurnContext));
            CombatBoardState combatBoardState = new CombatBoardState(m_currentRoom, m_spawner, m_movementService);

            foreach (Entity entity in sortedEntities)
                entity.TurnContext.StartBattle(combatBoardState);

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(m_currentTurnOrder, combatBoardState);
            OnBattleStarted?.Invoke(this, args);

            await ProcessBattle();
        }

        private async Task ProcessBattle()
        {
            while (!m_battleEnded)
            {
                EntityTurnContext turnContext = (EntityTurnContext)m_currentTurnOrder.Current;

                m_currentRoom.TryGetEntity(turnContext.TypedModel, out Entity entity);

                OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(entity);
                OnTurnSwitched?.Invoke(this, args);
                await turnContext.StartTurnTask();

                m_currentTurnOrder.Next();
            }
        }

        private void HandleEntityOnHealthChanged(EntityModel model, OnHealthChangedEventArgs args)
        {
            if (args.CurrentHealth <= 0)
                RemoveEntityFromConsideration(model);
        }

        private void RemoveEntityFromConsideration(EntityModel entityModel)
        {
            m_currentRoom.TryGetEntity(entityModel, out Entity entity);

            EventHandler<OnHealthChangedEventArgs> handler = m_healthHandlers[entityModel];
            entityModel.Health.OnHealthChanged -= handler;
            m_healthHandlers.Remove(entityModel);

            if (entityModel.IsPlayer)
            {
                GameObject.Destroy(entity.gameObject); // TODO: Use object pool instead of destroying
                End(isPlayerVictory: false);
            }
            else
            {
                m_currentTurnOrder.Remove(entity.TurnContext);
                m_currentRoom.RemoveEntityFromRoom(entity);

                OnEntityDiedEventArgs args = new OnEntityDiedEventArgs(entity);
                OnEntityDied?.Invoke(this, args);

                GameObject.Destroy(entity.gameObject); // TODO: Use object pool instead of destroying

                bool anyEnemiesInRoom = m_currentRoom.Models.Any(e => e.IsPlayer == false);
                if (!anyEnemiesInRoom)
                {
                    // Win
                    End(isPlayerVictory: true);
                }
            }
        }

        private void End(bool isPlayerVictory)
        {
            if (isPlayerVictory)
            {

            }
            else
            {
                // TODO: Handle player defeat
            }

            List<Entity> leftEntities = m_currentRoom.Entities.ToList();
            foreach (Entity entity in leftEntities)
            {
                m_currentRoom.RemoveEntityFromRoom(entity);
                entity.TurnContext.EndBattle();
            }

            m_currentTurnOrder.Clear();
            m_healthHandlers.Clear();

            OnBattleEndedEventArgs args = new OnBattleEndedEventArgs(isPlayerVictory);
            OnBattleEnded?.Invoke(this, args);

            m_battleEnded = true;
        }
    }
}