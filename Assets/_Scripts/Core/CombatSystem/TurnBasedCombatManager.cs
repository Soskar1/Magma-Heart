using MagmaHeart.Core.Dungeon;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnBasedCombatManager
    {
        private Tilemap m_corridors;
        private Room m_currentRoom;

        public void Initialize(Tilemap corridors)
        {
            m_corridors = corridors;
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