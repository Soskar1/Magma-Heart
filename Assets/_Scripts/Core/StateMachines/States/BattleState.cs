using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.SceneLoading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public class BattleState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;
        private readonly CameraController m_camera;
        private readonly HoverModeController m_hoverModeController;
        private readonly Battle m_battle;
        private readonly Player m_player;
        private readonly PlayerTurnContext m_turnContext;

        public BattleState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;
            m_battle = context.Battle;
            m_camera = context.CameraController;
            m_hoverModeController = context.HoverModeController;
            m_player = context.Player;
            m_turnContext = (PlayerTurnContext)m_player.TurnContext;
        }

        public async Task EnterAsync(StatePayload payload)
        {
            m_battle.OnBattleEnded += HandleOnBattleEnded;
            m_battle.OnTurnSwitched += HandleTurnSwitched;
            m_turnContext.OnCombatActionExecutionStarted += HandleOnCombatActionExecutionStarted;
            m_turnContext.OnCombatActionExecuted += HandleOnCombatActionExecuted;
            
            m_camera.EnableManualMovement(m_context.DungeonController.CurrentRoom.RoomModel.OccupiedSpace);
            m_player.CombatController.Enable();

            await m_battle.Start(m_context.DungeonController.CurrentRoom, m_player);
        }

        public Task ExitAsync()
        {
            m_camera.DisableManualMovement();
            m_hoverModeController.UseRaycastHover();

            m_battle.OnBattleEnded -= HandleOnBattleEnded;
            m_battle.OnTurnSwitched -= HandleTurnSwitched;
            m_turnContext.OnCombatActionExecutionStarted -= HandleOnCombatActionExecutionStarted;
            m_turnContext.OnCombatActionExecuted -= HandleOnCombatActionExecuted;

            m_player.CombatController.Disable();

            return Task.CompletedTask;
        }

        private async void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs e)
        {
            if (e.IsPlayerVictory)
                await m_stateMachine.FireTrigger(StateMachineTriggers.BattleWon);
            else
                await m_stateMachine.FireTrigger(StateMachineTriggers.BattleLost);
        }

        private void HandleOnCombatActionExecutionStarted() => m_hoverModeController.UseRaycastHover();

        private void HandleOnCombatActionExecuted() => m_hoverModeController.UseTileHover();

        private void HandleTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model.IsPlayer)
                m_hoverModeController.UseTileHover();
            else
                m_hoverModeController.UseRaycastHover();
        }
    }
}