using System.Collections.Generic;
using MagmaHeart.Core.CameraControls;
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
        private CameraController m_cameraController;
        private List<IDisplayable> m_combatUI;

        private Battle m_currentBattle;

        // TODO: think more about this class. Maybe we can implement it much more easier (use some kind of interface to switch states)
        public CombatStateSwitcher(Tilemap corridors, Player player, CameraController cameraController, Spawner spawner, List<IDisplayable> combatUI)
        {
            m_corridors = corridors;
            m_player = player;
            m_spawner = spawner;
            m_combatUI = combatUI;
            m_cameraController = cameraController;
        }

        public void EnterCombatState(Room room)
        {
            m_corridors.gameObject.SetActive(true);
            m_currentRoom = room;

            m_player.EnterCombat();
            m_cameraController.SwitchToTurnBasedCamera();

            List<ICombatController> entitiesInCombat = new List<ICombatController>() { m_player.TurnBasedPlayerBehaviour };

            for (int i = 0; i < 3; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                ICombatController spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                entitiesInCombat.Add(spawnedEntity);
            }

            m_currentBattle = new Battle(room, entitiesInCombat, m_combatUI);
            m_currentBattle.BattleEnded += ExitCombatState;
            m_currentBattle.Start();
        }

        private void ExitCombatState(object obj, BattleEndedEventArgs e)
        {
            m_currentBattle.BattleEnded -= ExitCombatState;
            m_currentBattle = null;

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

                m_cameraController.SwitchToActionCamera();

                Debug.Log("Player won the battle!");
            }
            else
            {
                // TODO: Handle player defeat
            }

        }
    }
}