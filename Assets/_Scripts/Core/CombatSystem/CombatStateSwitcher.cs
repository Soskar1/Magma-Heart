using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.NonPlayableCharacters;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatStateSwitcher
    {
        private Tilemap m_corridors;
        private Room m_currentRoom;
        private Player m_player;
        private Spawner m_spawner;
        private TurnOrderBuilder m_turnOrderBuilder;

        public CombatStateSwitcher(Tilemap corridors, Player player, Spawner spawner)
        {
            m_corridors = corridors;
            m_player = player;
            m_spawner = spawner;
            m_turnOrderBuilder = new TurnOrderBuilder();
        }

        public void EnterCombatState(Room room)
        {
            m_corridors.gameObject.SetActive(true);
            m_currentRoom = room;

            m_player.EnterCombat();

            List<ITurnController> entitiesInCombat = new List<ITurnController>() { m_player.TurnBasedPlayerBehaviour };

            for (int i = 0; i < 3; ++i) // TODO: Add difficulty to every room and determine how many enemies to spawn
            {
                Enemy spawnedEntity = m_spawner.SpawnEnemy(room.RoomTileData);
                entitiesInCombat.Add(spawnedEntity);
            }

            TurnOrder turnOrder = m_turnOrderBuilder.Build(entitiesInCombat);

            // TODO: Turn on combat HUD

            TurnSwitcher turnSwitcher = new TurnSwitcher(turnOrder);
            turnSwitcher.Start();
        }

        private void EndCombat()
        {
            m_corridors.gameObject.SetActive(false);
            m_player.ExitCombat();

            // TODO: Turn off combat HUD
        }
    }
}