using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.StateMachines
{
    public class ActionState : IState
    {
        private readonly PlayerController m_controller;
        private readonly HoverManager m_mouseHover;
        private readonly RaycastHoverHandler m_actionHoverHandler;

        public ActionState(PlayerController controller, HoverManager mouseHover)
        {
            m_controller = controller;
            m_mouseHover = mouseHover;
            m_actionHoverHandler = new RaycastHoverHandler();
        }

        public void Enter()
        {
            m_controller.Enable();
            m_mouseHover.SetHandler(m_actionHoverHandler);
        }

        public void Exit()
        {
            m_controller.Disable();
        }
    }
}