using MagmaHeart.Core.SceneLoading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines.States
{
    public class PrepareRoomState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;

        public PrepareRoomState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;
        }

        public async Task EnterAsync()
        {
            m_context.DungeonController.GenerateRoom();

            await m_context.RoomRenderer.OnRoomRendered;

            TravelStatePayload payload = new TravelStatePayload(TravelReason.EnterRoom);
            await m_stateMachine.FireTrigger(StateMachineTriggers.RoomPrepared, payload);
        }

        public Task PayloadEnterAsync(StatePayload payload)
        {
            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
