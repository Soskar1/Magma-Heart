using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.StateMachines
{
    public class ActionState : IState
    {
        private readonly PlayerController m_controller;
        private readonly MouseHover m_mouseHover;

        public ActionState(PlayerController controller, MouseHover mouseHover)
        {
            m_controller = controller;
            m_mouseHover = mouseHover;
        }

        public void Enter()
        {
            m_controller.Enable();
            m_mouseHover.UseRaycastHover();
        }

        public void Exit()
        {
            m_controller.Disable();
        }
    }
}