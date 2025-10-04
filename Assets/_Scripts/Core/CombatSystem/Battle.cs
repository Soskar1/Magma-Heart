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
        private readonly Room m_room;
        private readonly TurnSwitcher m_turnSwitcher;
        private readonly List<ICombatController> m_entities;

        public EventHandler<BattleEndedEventArgs> BattleEnded;

        public Battle(Room room, List<ICombatController> entities, TurnSwitcher turnSwitcher)
        {
            m_room = room;
            m_entities = entities;
            m_turnSwitcher = turnSwitcher;
        }

        public void Start()
        {
            foreach (ICombatController controller in m_entities)
            {
                controller.StartCombat(m_room);
                controller.Health.OnDeath += HandleEntityDeath;

                m_room.AddEntityToInspect(controller);
            }

            m_turnSwitcher.Start(m_entities);
        }

        private void HandleEntityDeath(object obj, EventArgs args)
        {
            Health health = obj as Health;
            health.OnDeath -= HandleEntityDeath;

            ICombatController controller = null;
            foreach (ICombatController entity in m_entities)
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

        private void End(bool isPlayerVictory) => BattleEnded?.Invoke(this, new BattleEndedEventArgs(isPlayerVictory, m_entities));
    }
}