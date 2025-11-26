using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.CombatSystem;
using MagmaHeart.Core.Entities.Models;
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
        private readonly List<ITurnSwitchListener> m_turnSwitchListeners;
        private readonly Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>> m_healthHandlers = new Dictionary<EntityModel, EventHandler<OnHealthChangedEventArgs>>();

        private Room m_currentRoom;
        private Dictionary<EntityModel, CombatController> m_currentEntitiesInBattle;
        private TurnOrder m_currentTurnOrder;
        
        public event EventHandler OnPlayerVictory;
        public event EventHandler<OnBattleStartedEventArgs> OnBattleStarted;
        private event EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;

        private bool m_battleEnded = false;

        public Battle(Entity player, Spawner spawner, List<ITurnSwitchListener> turnSwitchListeners)
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
            m_currentEntitiesInBattle = new Dictionary<EntityModel, CombatController>() { { m_player.Model, m_player.CombatController } };

            for (int i = 0; i < 2; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                Enemy spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                m_currentEntitiesInBattle.Add(spawnedEntity.Model, spawnedEntity.CombatController);
            }

            List<CombatController> combatControllers = m_currentEntitiesInBattle.Values.ToList();
            IEnumerable<CombatController> sortedCombatControllers = IniciativeRollSort.SortByRollingIniciative(combatControllers);

            foreach (CombatController combatController in sortedCombatControllers)
            {
                Entity entity = combatController.Entity;
                m_currentRoom.AddEntityToInspect(entity);

                EventHandler<OnHealthChangedEventArgs> handler = new EventHandler<OnHealthChangedEventArgs>((sender, args) =>
                {
                    HandleEntityOnHealthChanged(entity.Model, args);
                });

                m_healthHandlers[entity.Model] = handler;
                entity.Health.OnHealthChanged += handler;
            }

            m_currentTurnOrder = new TurnOrder(combatControllers);
            CombatBoardState combatBoardState = new CombatBoardState(m_currentRoom);

            OnBattleStartedEventArgs args = new OnBattleStartedEventArgs(m_currentTurnOrder, combatBoardState);
            OnBattleStarted?.Invoke(this, args);

            foreach (CombatController controller in combatControllers)
                controller.StartBattle(combatBoardState);

            await ProcessBattle();
        }

        private async Task ProcessBattle()
        {
            while (!m_battleEnded)
            {
                CombatController combatController = (CombatController)m_currentTurnOrder.Current;

                OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(combatController.Entity);
                OnTurnSwitched?.Invoke(this, args);
                await combatController.StartTurnTask();

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
            CombatController combatController = m_currentEntitiesInBattle[entityModel];

            EventHandler<OnHealthChangedEventArgs> handler = m_healthHandlers[entityModel];
            entityModel.Health.OnHealthChanged -= handler;
            m_healthHandlers.Remove(entityModel);

            if (entityModel.IsPlayer)
            {
                End(isPlayerVictory: false);
            }
            else
            {
                m_currentTurnOrder.Remove(combatController);
                m_currentEntitiesInBattle.Remove(entityModel);
                m_currentRoom.RemoveEntityFromRoom(combatController.Entity);

                bool anyEnemiesInRoom = m_currentEntitiesInBattle.Values.Any(e => e.Owner.IsPlayer == false);
                if (!anyEnemiesInRoom)
                {
                    // Win
                    End(isPlayerVictory: true);
                }
            }

            GameObject.Destroy(combatController.Entity.gameObject); // TODO: Use object pool instead of destroying
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

            foreach (CombatController combatController in m_currentEntitiesInBattle.Values)
            {
                m_currentRoom.RemoveEntityFromRoom(combatController.Entity);
                combatController.EndBattle();
            }

            m_currentTurnOrder.Clear();
            m_currentEntitiesInBattle.Clear();
            m_healthHandlers.Clear();

            m_battleEnded = true;
        }
    }
}