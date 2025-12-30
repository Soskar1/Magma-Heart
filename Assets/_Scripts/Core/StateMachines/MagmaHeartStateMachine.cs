using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachines.States;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public enum StateMachineTriggers
    {
        StartupComplete,
        RoomPrepared,
        TravelCompleted_Enter,
        TravelCompleted_Exit,
        BattleLost,
        BattleWon,
        RewardPicked
    }

    public class MagmaHeartStateMachine
    {
        private readonly StateMachine<StateMachineTriggers> m_stateMachine;

        private readonly StartupState m_startupState;
        private readonly PrepareRoomState m_prepareRoomState;
        private readonly TravelState m_travelState;
        private readonly BattleState m_battleState;
        private readonly RewardState m_rewardState;
        private readonly GameOverState m_gameOverState;
        
        public MagmaHeartStateMachine(MagmaHeartContext context, int travelSpeed)
        {
            m_startupState = new StartupState(this, context);
            m_prepareRoomState = new PrepareRoomState(this, context);
            m_travelState = new TravelState(this, context, travelSpeed);
            m_battleState = new BattleState(this, context);
            m_rewardState = new RewardState(this, context);
            m_gameOverState = new GameOverState();

            m_stateMachine = new StateMachine<StateMachineTriggers>(m_startupState);
            m_stateMachine.Configure(m_startupState)
                .Permit(StateMachineTriggers.StartupComplete, m_prepareRoomState)
                .OnEntryAsync(m_startupState.EnterAsync)
                .OnExitAsync(m_startupState.ExitAsync);

            m_stateMachine.Configure(m_prepareRoomState)
                .Permit(StateMachineTriggers.RoomPrepared, m_travelState)
                .OnEntryAsync(m_prepareRoomState.EnterAsync)
                .OnExitAsync(m_prepareRoomState.ExitAsync);

            m_stateMachine.Configure(m_travelState)
                .Permit(StateMachineTriggers.TravelCompleted_Enter, m_battleState)
                .Permit(StateMachineTriggers.TravelCompleted_Exit, m_prepareRoomState)
                .OnEntryAsync(m_travelState.EnterAsync)
                .OnExitAsync(m_travelState.ExitAsync);

            m_stateMachine.Configure(m_battleState)
                .Permit(StateMachineTriggers.BattleWon, m_rewardState)
                .Permit(StateMachineTriggers.BattleLost, m_gameOverState)
                .OnEntryAsync(m_battleState.EnterAsync)
                .OnExitAsync(m_battleState.ExitAsync);

            m_stateMachine.Configure(m_rewardState)
                .Permit(StateMachineTriggers.RewardPicked, m_travelState)
                .OnEntryAsync(m_rewardState.EnterAsync)
                .OnExitAsync(m_rewardState.ExitAsync);

            m_stateMachine.Configure(m_gameOverState)
                .OnEntryAsync(m_gameOverState.EnterAsync)
                .OnExitAsync(m_gameOverState.ExitAsync);
        }

        public async Task Start() => await m_startupState.EnterAsync();

        public async Task FireTrigger(StateMachineTriggers trigger, StatePayload payload = null) => await m_stateMachine.FireAsync(trigger, payload);
    }
}