using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.StateMachines;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class CombatAltar : MonoBehaviour, IInteractable
    {
        private GameStateMachine m_stateMachine;
        private Room m_room;

        public void Initialize(Room room, GameStateMachine stateMachine)
        {
            m_room = room;
            m_stateMachine = stateMachine;
        }

        public void Interact()
        {
            Destroy(gameObject);
            m_stateMachine.ChangeState(StateMachineStates.Combat, m_room);
        }
    }
}

