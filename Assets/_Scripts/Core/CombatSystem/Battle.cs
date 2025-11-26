using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.CombatSystem;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private readonly EntityPresenter m_player;
        private readonly Spawner m_spawner;
        private readonly List<ITurnSwitchListener> m_turnSwitchListeners;
        private readonly Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>> m_healthHandlers = new Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>>();

        private Room m_currentRoom;
        private TurnOrder m_currentTurnOrder;
        
        public event EventHandler OnPlayerVictory;
        public event EventHandler<OnBattleStartedEventArgs> OnBattleStarted;
        private event EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;

        private bool m_battleEnded = false;

        public Battle(EntityPresenter player, Spawner spawner, List<ITurnSwitchListener> turnSwitchListeners)
        {
            m_player = player;
            m_spawner = spawner;
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

            m_currentRoom.AddEntityToInspect(m_player);
            for (int i = 0; i < 1; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                Enemy spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                m_currentRoom.AddEntityToInspect(spawnedEntity);

                EventHandler<OnHealthChangedEventArgs> handler = new EventHandler<OnHealthChangedEventArgs>((sender, args) =>
                {
                    HandleEntityOnHealthChanged(spawnedEntity.Model, args);
                });

                m_healthHandlers[spawnedEntity.Model] = handler;
                spawnedEntity.Health.OnHealthChanged += handler;
            }

            IEnumerable<EntityPresenter> sortedEntities = IniciativeRollSort.SortByRollingIniciative(m_currentRoom.Entities);

            m_currentTurnOrder = new TurnOrder(sortedEntities.Select(e => e.TurnContext));
            CombatBoardState combatBoardState = new CombatBoardState(m_currentRoom);

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(m_currentTurnOrder, combatBoardState);
            OnBattleStarted?.Invoke(this, args);

            foreach (EntityPresenter entity in sortedEntities)
                entity.TurnContext.StartBattle(combatBoardState);

            await ProcessBattle();
        }

        private async Task ProcessBattle()
        {
            while (!m_battleEnded)
            {
                EntityTurnContext turnContext = (EntityTurnContext)m_currentTurnOrder.Current;

                m_currentRoom.TryGetEntityPresenter(turnContext.TypedModel, out EntityPresenter entity);

                OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(entity);
                OnTurnSwitched?.Invoke(this, args);
                await turnContext.StartTurnTask();

                m_currentTurnOrder.Next();
            }
        }

        private void HandleEntityOnHealthChanged(EntityModel model, OnHealthChangedEventArgs args)
        {
            if (args.CurrentHealth < 0)
                RemoveEntityFromConsideration(model);
        }

        private void RemoveEntityFromConsideration(EntityModel entityModel)
        {
            m_currentRoom.TryGetEntityPresenter(entityModel, out EntityPresenter entity);

            EventHandler<OnHealthChangedEventArgs> handler = m_healthHandlers[entityModel];
            entityModel.Health.OnHealthChanged -= handler;
            m_healthHandlers.Remove(entityModel);

            if (entityModel.IsPlayer)
            {
                End(isPlayerVictory: false);
            }
            else
            {
                m_currentTurnOrder.Remove(entity.TurnContext);
                m_currentRoom.RemoveEntityFromRoom(entity);

                bool anyEnemiesInRoom = m_currentRoom.Models.Any(e => e.IsPlayer == false);
                if (!anyEnemiesInRoom)
                {
                    // Win
                    End(isPlayerVictory: true);
                }
            }

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

            List<EntityPresenter> leftEntities = m_currentRoom.Entities.ToList();
            foreach (EntityPresenter entity in leftEntities)
            {
                m_currentRoom.RemoveEntityFromRoom(entity);
                entity.TurnContext.EndBattle();
            }

            m_currentTurnOrder.Clear();
            m_healthHandlers.Clear();

            m_battleEnded = true;
        }
    }
}