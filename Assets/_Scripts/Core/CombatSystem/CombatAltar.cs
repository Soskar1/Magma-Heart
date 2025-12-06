using MagmaHeart.Core.BoardStateSystem;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatAltar : MonoBehaviour, IInteractable
    {
        private Room m_room;
        private Battle m_battle;

        public void Initialize(Room room, Battle battle)
        {
            m_room = room;
            m_battle = battle;
        }

        public async void Interact()
        {
            Destroy(gameObject);
            await m_battle.Start(m_room);
        }
    }
}

