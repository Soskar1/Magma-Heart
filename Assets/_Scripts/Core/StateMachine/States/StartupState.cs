using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.StateMachine;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachine.States
{
    public class StartupState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;

        public StartupState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_context = context;
            m_stateMachine = stateMachine;
        }

        public async Task EnterAsync(StatePayload payload = null)
        {
            Player player = m_context.Player;
            DungeonController dungeon = m_context.DungeonController;

            player.gameObject.SetActive(true);
            m_context.HoverModeController.UseRaycastHover();

            await m_stateMachine.FireTrigger(StateMachineTriggers.StartupComplete);
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
