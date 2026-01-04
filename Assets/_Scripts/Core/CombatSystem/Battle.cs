using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private readonly Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>> m_healthHandlers = new Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>>();
        private readonly TurnContext m_turnContext;
        private readonly MagmaHeartServices m_services;

        private Room m_currentRoom;
        private TurnOrder m_currentTurnOrder;
        private CombatBoardState m_currentBoardState;

        public event EventHandler<OnBattleStartedEventArgs> OnBattleStarted;
        public event EventHandler<OnBattleEndedEventArgs> OnBattleEnded;
        public event EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;
        public event EventHandler<OnEntityDiedEventArgs> OnEntityDied;

        private bool m_battleEnded = false;

        private CancellationTokenSource m_cancellationTokenSource;

        public Battle(MagmaHeartServices services, TurnContext turnContext)
        {
            m_services = services;
            m_turnContext = turnContext;
        }

        public async Task Start(Room room, IEnumerable<Entity> entities)
        {
            m_battleEnded = false;
            m_currentRoom = room;

            IEnumerable<Entity> sortedEntities = IniciativeRollSort.SortByRollingIniciative(entities);
            foreach (Entity entity in sortedEntities)
            {
                m_currentRoom.AddEntityToInspect(entity);

                // TODO: handle player properly
                if (entity.Model.IsPlayer)
                    continue;

                EventHandler<OnHealthChangedEventArgs> handler = new EventHandler<OnHealthChangedEventArgs>((sender, args) =>
                {
                    HandleEntityOnHealthChanged(entity.Model, args);
                });

                m_healthHandlers[entity.Model] = handler;
                entity.Health.OnHealthChanged += handler;
            }

            m_currentTurnOrder = new TurnOrder(sortedEntities);
            m_currentBoardState = new CombatBoardState(m_currentRoom, m_services);

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(m_currentTurnOrder, m_currentBoardState);
            OnBattleStarted?.Invoke(this, args);

            await ProcessBattle();
        }

        private async Task ProcessBattle()
        {
            while (!m_battleEnded)
            {
                Entity entity = m_currentTurnOrder.Current;

                OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(entity);
                OnTurnSwitched?.Invoke(this, args);

                m_cancellationTokenSource = new CancellationTokenSource();
                await m_turnContext.StartTurnAsync(m_currentBoardState, entity.Model, m_cancellationTokenSource.Token);
                await entity.TurnController.StartTurn(m_currentBoardState, m_currentTurnOrder);

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
                m_currentTurnOrder.Remove(entity);
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
                entity.TurnController.EndBattle();
            }

            m_currentTurnOrder.Clear();
            m_healthHandlers.Clear();

            OnBattleEndedEventArgs args = new OnBattleEndedEventArgs(isPlayerVictory);
            OnBattleEnded?.Invoke(this, args);

            m_battleEnded = true;
        }
    }
}