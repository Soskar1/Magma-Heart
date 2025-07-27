using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatAltar : MonoBehaviour, IInteractable
    {
        private TurnBasedCombatManager m_combatManager;
        private Room m_room;

        public void Initialize(Room room, TurnBasedCombatManager combatManager)
        {
            m_room = room;
            m_combatManager = combatManager;
        }

        public void Interact()
        {
            Destroy(gameObject);
            m_combatManager.StartCombat(m_room);
        }
    }
}

