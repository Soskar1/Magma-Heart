using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatStateSwitcher
    {
        private readonly TurnBasedPlayerBehaviour m_player;
        private readonly TurnSwitcher m_turnSwitcher;
        private readonly Spawner m_spawner;
        private readonly List<ICombatStateListener> m_combatStateListeners;
        private readonly List<ICombatTurnSwitchListener> m_turnSwitchListeners;

        public CombatStateSwitcher(TurnBasedPlayerBehaviour player, Spawner spawner, List<ICombatStateListener> combatStateListeners, List<ICombatTurnSwitchListener> turnSwitchListeners)
        {
            m_player = player;
            m_spawner = spawner;
            m_combatStateListeners = combatStateListeners;
            m_turnSwitchListeners = turnSwitchListeners;

            m_turnSwitcher = new TurnSwitcher();
        }

        public void EnterCombatState(Room room)
        {
            foreach (ICombatStateListener listener in m_combatStateListeners)
                listener.EnterCombatState();

            List<ICombatController> entitiesInCombat = new List<ICombatController>() { m_player };

            for (int i = 0; i < 3; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                ICombatController spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                entitiesInCombat.Add(spawnedEntity);
            }

            IEnumerable<ICombatController> sortedEntities = IniciativeRollSort.SortByRollingIniciative(entitiesInCombat);

            foreach (ICombatTurnSwitchListener listener in m_turnSwitchListeners)
                m_turnSwitcher.OnTurnSwitched += listener.HandleOnTurnSwitched;

            Battle battle = new Battle(room, entitiesInCombat, m_turnSwitcher);
            battle.BattleEnded += ExitCombatState;
            battle.Start();
        }

        private void ExitCombatState(object obj, BattleEndedEventArgs e)
        {
            Battle battle = obj as Battle;

            foreach (ICombatTurnSwitchListener listener in m_turnSwitchListeners)
                m_turnSwitcher.OnTurnSwitched -= listener.HandleOnTurnSwitched;

            battle.BattleEnded -= ExitCombatState;

            if (e.IsPlayerVictory)
            {
                e.BattleSurvivors.ForEach(survivor =>
                {
                    survivor.NextTurn = null;
                    survivor.EndTurn();
                });

                Debug.Log("Player won the battle!");
            }
            else
            {
                // TODO: Handle player defeat
            }

            foreach (ICombatStateListener listener in m_combatStateListeners)
                listener.ExitCombatState();
        }
    }
}