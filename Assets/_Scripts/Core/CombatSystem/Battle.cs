using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class Battle
    {
        private Room m_room;
        private List<ICombatController> m_entities;
        private List<IDisplayable> m_combatUI;

        public EventHandler<BattleEndedEventArgs> BattleEnded;

        public Battle(Room room, List<ICombatController> entities, List<IDisplayable> combatUI)
        {
            m_room = room;
            m_entities = entities;
            m_combatUI = combatUI;
        }

        public void Start()
        {
            TurnOrder turnOrder = TurnOrderBuilder.Build(m_entities);

            // TODO: Turn on combat HUD

            TurnSwitcher turnSwitcher = new TurnSwitcher(turnOrder, m_combatUI);

            foreach (ICombatController controller in m_entities)
            {
                controller.StartCombat(m_room);
                m_room.AddEntityToInspect(controller);

                void HandleEntityOnDeathEvent(object obj, EventArgs args)
                {
                    controller.Health.OnDeath -= HandleEntityOnDeathEvent;

                    if (controller.IsPlayableCharacter)
                    {
                        End(isPlayerVictory: false);
                    }
                    else
                    {
                        turnOrder.Remove(controller);
                        m_entities.Remove(controller);
                        m_room.RemoveEntityFromRoom(controller);

                        bool anyEnemiesInRoom = m_entities.Any(e => e.IsPlayableCharacter == false);
                        if (!anyEnemiesInRoom)
                        {
                            // Win
                            End(isPlayerVictory: true);
                        }
                    }

                    Debug.Log($"Entity died: {controller.Transform.gameObject.name}");
                    GameObject.Destroy(controller.Transform.gameObject); // TODO: Use object pool instead of destroying
                }

                controller.Health.OnDeath += HandleEntityOnDeathEvent;
            }

            turnSwitcher.Start();
        }

        private void End(bool isPlayerVictory) => BattleEnded?.Invoke(this, new BattleEndedEventArgs(isPlayerVictory, m_entities));
    }

    public class BattleEndedEventArgs : EventArgs
    {
        public List<ICombatController> BattleSurvivors { get; }
        public bool IsPlayerVictory { get; }
        public BattleEndedEventArgs(bool isPlayerVictory, List<ICombatController> entities)
        {
            IsPlayerVictory = isPlayerVictory;
            BattleSurvivors = entities;
        }
    }
}