using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using UnityEngine.Tilemaps;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatStateSwitcher
    {
        private Tilemap m_corridors;
        private Room m_currentRoom;
        private Player m_player;

        public CombatStateSwitcher(Tilemap corridors, Player player)
        {
            m_corridors = corridors;
            m_player = player;
        }

        public void EnterCombatState(Room room)
        {
            m_corridors.gameObject.SetActive(true);
            m_currentRoom = room;

            m_currentRoom.ShowCombatTiles();
            m_player.EnterCombat();
        }

        private void EndCombat()
        {
            m_corridors.gameObject.SetActive(false);
            m_player.ExitCombat();
        }
    }
}