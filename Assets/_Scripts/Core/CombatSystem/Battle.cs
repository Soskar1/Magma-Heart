using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.CombatSystem;
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
                entity.Health.OnDeath += HandleEntityDeath;
                m_currentRoom.AddEntityToInspect(entity);
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

        private void HandleEntityDeath(object obj, OnDeathEventArgs args)
        {
            EntityModel model = args.Model;
            CombatController combatController = m_currentEntitiesInBattle[model];
            model.Health.OnDeath -= HandleEntityDeath;

            if (model.IsPlayer)
            {
                End(isPlayerVictory: false);
            }
            else
            {
                m_currentTurnOrder.Remove(combatController);
                m_currentEntitiesInBattle.Remove(model);
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

            m_battleEnded = true;
        }
    }
}