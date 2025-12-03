using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.StateMachines
{
    public class CombatState : IState
    {
        private readonly CameraController m_camera;
        private readonly DungeonGrid m_grid;

        public CombatState(CameraController camera, DungeonGrid grid)
        {
            m_camera = camera;
            m_grid = grid;
        }

        public void Enter()
        {
            m_camera.SwitchToTurnBasedCamera();
            m_grid.Corridors.gameObject.SetActive(true);
        }

        public void Exit()
        {
            m_camera.SwitchToActionCamera();
            m_grid.Corridors.gameObject.SetActive(false);
        }
    }
}