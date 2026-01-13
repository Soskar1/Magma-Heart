using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Input.Mouse;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.TutorialSystem;
using MagmaHeart.StateMachine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachine
{
    public class BattleState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;
        private readonly CameraController m_camera;
        private readonly HoverModeController m_hoverModeController;
        private readonly Battle m_battle;
        private readonly Entity m_player;
        private readonly PlayerTurnController m_turnController;

        public BattleState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;
            m_battle = context.BattleContext.Battle;
            m_camera = context.CameraController;
            m_hoverModeController = context.HoverModeController;
            m_player = context.Player;
            m_turnController = (PlayerTurnController)m_player.TurnController;
        }

        public async Task EnterAsync(StatePayload payload)
        {
            m_battle.OnBattleEnded += HandleOnBattleEnded;
            m_battle.OnTurnSwitched += HandleTurnSwitched;
            m_turnController.OnCombatActionExecutionStarted += HandleOnCombatActionExecutionStarted;
            m_turnController.OnCombatActionExecuted += HandleOnCombatActionExecuted;

            Room room = m_context.DungeonController.CurrentRoom;
            m_camera.EnableManualMovement(room.RoomModel.OccupiedSpace);
            m_turnController.Enable();

            IEnumerable<Entity> entities = m_context.BattleContext.Initializer.InitializeBattle(room, m_player);
            await m_battle.Start(room, entities);
        }

        public Task ExitAsync()
        {
            m_camera.DisableManualMovement();
            m_hoverModeController.UseRaycastHover();

            m_battle.OnBattleEnded -= HandleOnBattleEnded;
            m_battle.OnTurnSwitched -= HandleTurnSwitched;
            m_turnController.OnCombatActionExecutionStarted -= HandleOnCombatActionExecutionStarted;
            m_turnController.OnCombatActionExecuted -= HandleOnCombatActionExecuted;

            m_turnController.Disable();

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
            {
                m_hoverModeController.UseTileHover();

                m_context.Tutorial.Model.TrySetFlag(TutorialFlags.CameraMovementExplained);
                m_context.Tutorial.Model.TrySetFlag(TutorialFlags.CombatSystemExplained);
            }
            else
            {
                m_hoverModeController.UseRaycastHover();
            }
        }
    }
}