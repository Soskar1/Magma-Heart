using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Presentation;

namespace MagmaHeart.Core.StateMachines
{
    public class CombatState : IState
    {
        private readonly CameraController m_camera;
        private readonly DungeonGrid m_grid;
        private readonly MouseHover m_mouseHover;

        public CombatState(CameraController camera, DungeonGrid grid, MouseHover mouseHover)
        {
            m_camera = camera;
            m_grid = grid;
            m_mouseHover = mouseHover;
        }

        public void Enter()
        {
            m_camera.SwitchToTurnBasedCamera();
            m_grid.Corridors.gameObject.SetActive(true);
            m_mouseHover.UseCombatHover();
        }

        public void Exit()
        {
            m_camera.SwitchToActionCamera();
            m_grid.Corridors.gameObject.SetActive(false);
            m_mouseHover.UseRaycastHover();
        }
    }
}