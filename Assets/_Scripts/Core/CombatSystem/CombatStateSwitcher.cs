using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatStateSwitcher
    {
        private Tilemap m_corridors;
        private Room m_currentRoom;
        private Player m_player;
        private Spawner m_spawner;
        private List<IDisplayable> m_combatUI;

        public CombatStateSwitcher(Tilemap corridors, Player player, Spawner spawner, List<IDisplayable> combatUI)
        {
            m_corridors = corridors;
            m_player = player;
            m_spawner = spawner;
            m_combatUI = combatUI;
        }

        public void EnterCombatState(Room room)
        {
            m_corridors.gameObject.SetActive(true);
            m_currentRoom = room;

            m_player.EnterCombat();

            List<ICombatController> entitiesInCombat = new List<ICombatController>() { m_player.TurnBasedPlayerBehaviour };

            for (int i = 0; i < 3; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                ICombatController spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                entitiesInCombat.Add(spawnedEntity);
            }

            Battle battle = new Battle(room, entitiesInCombat, m_combatUI);
            battle.BattleEnded += ExitCombatState;
            battle.Start();
        }

        private void ExitCombatState(object obj, BattleEndedEventArgs e)
        {
            foreach (IDisplayable displayable in m_combatUI)
                displayable.Hide();

            if (e.IsPlayerVictory)
            {
                e.BattleSurvivors.ForEach(survivor =>
                {
                    survivor.NextTurn = null;
                    survivor.EndTurn();
                });

                m_corridors.gameObject.SetActive(false);
                m_player.ExitCombat();

                Debug.Log("Player won the battle!");
            }
            else
            {
                // TODO: Handle player defeat
            }

        }
    }
}