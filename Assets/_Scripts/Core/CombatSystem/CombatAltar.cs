using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatAltar : MonoBehaviour, IInteractable
    {
        private CombatStateSwitcher m_combatStateSwitcher;
        private Room m_room;

        public void Initialize(Room room, CombatStateSwitcher combatStateSwitcher)
        {
            m_room = room;
            m_combatStateSwitcher = combatStateSwitcher;
        }

        public void Interact()
        {
            Destroy(gameObject);
            m_combatStateSwitcher.EnterCombatState(m_room);
        }
    }
}

