using MagmaHeart.Core.SceneLoading;
using MagmaHeart.StateMachine;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachine.States
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

        public async Task EnterAsync(StatePayload payload)
        {
            m_context.DungeonController.GenerateNextRoom();

            await m_context.RoomRenderer.OnRoomRendered;

            TravelStatePayload travelPayload = new TravelStatePayload(TravelReason.EnterRoom);
            await m_stateMachine.FireTrigger(StateMachineTriggers.RoomPrepared, travelPayload);
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
