using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.StateMachines
{
    public class CombatState : IState
    {
        private readonly CameraController m_camera;
        private readonly DungeonGrid m_grid;
        private readonly HoverManager m_hoverManager;
        private readonly CombatHoverHandler m_hoverHandler;

        public CombatState(CameraController camera, DungeonGrid grid, HoverManager hoverManager, CombatHoverHandler combatHoverHandler)
        {
            m_camera = camera;
            m_grid = grid;
            m_hoverManager = hoverManager;
            m_hoverHandler = combatHoverHandler;
        }

        public void Enter()
        {
            m_camera.SwitchToTurnBasedCamera();
            m_grid.Corridors.gameObject.SetActive(true);

            m_hoverManager.SetHandler(m_hoverHandler);
        }

        public void Exit()
        {
            m_hoverManager.SetHandler(null);
            m_camera.SwitchToActionCamera();
            m_grid.Corridors.gameObject.SetActive(false);
        }
    }
}