using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input.Mouse;

namespace MagmaHeart.Core.StateMachines
{
    public class ActionState : IState
    {
        private readonly PlayerController m_controller;
        private readonly HoverModeController m_hoverModeController;

        public ActionState(PlayerController controller, HoverModeController hoverModeController)
        {
            m_controller = controller;
            m_hoverModeController = hoverModeController;
        }

        public void Enter()
        {
            m_controller.Enable();
            m_hoverModeController.UseRaycastHover();
        }

        public void Exit()
        {
            m_controller.Disable();
        }
    }
}