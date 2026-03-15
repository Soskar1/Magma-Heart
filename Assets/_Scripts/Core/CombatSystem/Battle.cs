using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private readonly Dictionary<int, EventHandler<OnParameterValueChangedEventArgs>> m_healthHandlers = new Dictionary<int, EventHandler<OnParameterValueChangedEventArgs>>();

        private Room m_currentRoom;
        private TurnOrder m_currentTurnOrder;

        public event EventHandler<OnBattleStartedEventArgs> OnBattleStarted;
        public event EventHandler<OnBattleEndedEventArgs> OnBattleEnded;
        public event EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;
        public event EventHandler<OnEntityDiedEventArgs> OnEntityDied;

        private bool m_battleEnded = false;

        private CancellationTokenSource m_cancellationTokenSource;

        private readonly PlayerTurnController m_playerTurnController;
        private readonly EnemyTurnController m_enemyTurnController;
        private readonly EffectDispatcher m_effectDispatcher;
        private readonly IStartOfTurnEffectFactory m_startOfTurnEffectFactory;
        private readonly GameWorld m_world;

        public Battle(PlayerTurnController playerTurnController, EnemyTurnController enemyTurnController, EffectDispatcher effectDispatcher, IStartOfTurnEffectFactory startOfTurnEffectFactory, GameWorld world)
        {
            m_playerTurnController = playerTurnController;
            m_enemyTurnController = enemyTurnController;

            m_effectDispatcher = effectDispatcher;
            m_startOfTurnEffectFactory = startOfTurnEffectFactory;
            m_world = world;
        }

        public async Task Start(Room room, List<Entity> entities)
        {
            m_battleEnded = false;
            m_currentRoom = room;

            IEnumerable<Entity> sortedEntities = IniciativeRollSort.SortByRollingIniciative(entities.Skip(1));
            sortedEntities = sortedEntities.Prepend(entities.First()); // Player always goes first
            foreach (Entity entity in sortedEntities)
            {
                m_world.AddEntity(entity);

                EventHandler<OnParameterValueChangedEventArgs> handler = new EventHandler<OnParameterValueChangedEventArgs>((sender, args) =>
                {
                    HandleEntityOnHealthChanged(entity.Model, args);
                });

                m_healthHandlers[entity.Model.Id] = handler;
                entity.Health.OnParameterValueChanged += handler;
            }

            m_currentTurnOrder = new TurnOrder(sortedEntities);

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(m_currentTurnOrder, m_currentRoom);
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

                IReadOnlyList<AbilityEffect> effects = m_startOfTurnEffectFactory.CreateStartOfTurnEffects(m_world, entity.Model.Id);
                foreach (AbilityEffect effect in effects)
                    m_effectDispatcher.Apply(m_world, effect);

                if (entity.Model.IsPlayer)
                    await m_playerTurnController.StartTurn(entity.Model);
                else
                    await m_enemyTurnController.StartTurn(m_currentRoom, m_currentTurnOrder);

                if (!m_battleEnded)
                    m_currentTurnOrder.Next();
            }
        }

        private void HandleEntityOnHealthChanged(EntityModel model, OnParameterValueChangedEventArgs args)
        {
            if (args.NewValue <= 0)
                HandleEntityDeath(model);
        }

        private void HandleEntityDeath(EntityModel entityModel)
        {
            m_world.TryGetEntity(entityModel.Id, out Entity entity);
            m_world.RemoveEntity(entityModel.Id);

            EventHandler<OnParameterValueChangedEventArgs> handler = m_healthHandlers[entityModel.Id];
            entityModel.Health.OnParameterValueChanged -= handler;
            m_healthHandlers.Remove(entityModel.Id);

            if (entityModel.IsPlayer)
            {
                GameObject.Destroy(entity.gameObject); // TODO: Use object pool instead of destroying
                End(isPlayerVictory: false);
            }
            else
            {
                m_currentTurnOrder.Remove(entity);

                OnEntityDiedEventArgs args = new OnEntityDiedEventArgs(entity);
                OnEntityDied?.Invoke(this, args);

                GameObject.Destroy(entity.gameObject); // TODO: Use object pool instead of destroying

                bool anyEnemiesInRoom = m_currentRoom.Entities.Any(e => e.Model.IsPlayer == false);
                if (!anyEnemiesInRoom)
                {
                    // Win
                    End(isPlayerVictory: true);
                }
            }
        }

        private void End(bool isPlayerVictory)
        {
            List<Entity> leftEntities = m_currentRoom.Entities.ToList();
            m_enemyTurnController.EndBattle();
            m_playerTurnController.Disable();

            foreach (Entity entity in leftEntities)
            {
                if (m_healthHandlers.TryGetValue(entity.Model.Id, out var handler))
                    entity.Model.Health.OnParameterValueChanged -= handler;

                // Maybe not the best place to put it, but it works...
                entity.Energy.Reset();
            }

            m_currentTurnOrder.Clear();
            m_healthHandlers.Clear();

            OnBattleEndedEventArgs args = new OnBattleEndedEventArgs(isPlayerVictory);
            OnBattleEnded?.Invoke(this, args);

            m_battleEnded = true;
        }
    }
}