using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input.Mouse;

namespace MagmaHeart.Core.StateMachines
{
    public class CombatState : IState
    {
        private readonly CameraController m_camera;
        private readonly DungeonGrid m_grid;
        private readonly HoverModeController m_hoverModeController;
        private readonly Battle m_battle;
        private readonly PlayerTurnContext m_player;

        public CombatState(CameraController camera, DungeonGrid grid, HoverModeController hoverModeController, Battle battle, PlayerTurnContext player)
        {
            m_camera = camera;
            m_grid = grid;
            m_hoverModeController = hoverModeController;
            m_battle = battle;
            m_player = player;
        }

        public void Enter()
        {
            m_camera.SwitchToTurnBasedCamera();
            m_grid.Corridors.gameObject.SetActive(true);
            
            m_hoverModeController.UseTileHover();

            m_battle.OnTurnSwitched += HandleTurnSwitched;
            m_player.OnCombatActionExecutionStarted += HandleOnCombatActionExecutionStarted;
            m_player.OnCombatActionExecuted += HandleOnCombatActionExecuted;
        }

        public void Exit()
        {
            m_camera.SwitchToActionCamera();
            m_grid.Corridors.gameObject.SetActive(false);
            m_hoverModeController.UseRaycastHover();

            m_battle.OnTurnSwitched -= HandleTurnSwitched;
            m_player.OnCombatActionExecutionStarted -= HandleOnCombatActionExecutionStarted;
            m_player.OnCombatActionExecuted -= HandleOnCombatActionExecuted;
        }

        private void HandleOnCombatActionExecutionStarted() => m_hoverModeController.UseRaycastHover();

        private void HandleOnCombatActionExecuted() => m_hoverModeController.UseTileHover();

        private void HandleTurnSwitched(object s, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model.IsPlayer)
                m_hoverModeController.UseTileHover();
            else
                m_hoverModeController.UseRaycastHover();
        }
    }
}