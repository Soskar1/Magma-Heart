using MagmaHeart.Core.Entities.PlayableCharacters;
using System.Collections.Generic;

namespace MagmaHeart.Core.StateMachines
{
    public class ActionState : IState
    {
        private readonly PlayerController m_controller;

        public ActionState(PlayerController controller)
        {
            m_controller = controller;
        }

        public void Enter()
        {
            m_controller.Enable();
        }

        public void Exit()
        {
            m_controller.Disable();
        }
    }
}