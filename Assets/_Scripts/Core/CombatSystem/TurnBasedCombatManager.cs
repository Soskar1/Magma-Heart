using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnBasedCombatManager
    {
        private Tilemap m_corridors;
        private Room m_currentRoom;
        private Player m_player;
        private PlayerNonCombatBehaviour m_nonCombatBehaviour;
        private PlayerCombatBehaviour m_combatBehaviour;

        private UserInput m_userInput;

        public TurnBasedCombatManager(Tilemap corridors)
        {
            m_corridors = corridors;
            m_userInput = new UserInput();

            // m_nonCombatBehaviour = new PlayerNonCombatBehaviour(m_userInput, m_player.Movement, );
        }

        public void StartCombat(Room room)
        {
            m_corridors.gameObject.SetActive(true);
            m_currentRoom = room;

            m_currentRoom.ShowCombatTiles();
        }

        private void EndCombat()
        {
            m_corridors.gameObject.SetActive(false);
        }
    }
}