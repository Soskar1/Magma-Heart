using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachine.States;
using MagmaHeart.StateMachine;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachine
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
        
        public MagmaHeartStateMachine(MagmaHeartContext context)
        {
            m_startupState = new StartupState(this, context);
            m_prepareRoomState = new PrepareRoomState(this, context);
            m_travelState = new TravelState(this, context);
            m_battleState = new BattleState(this, context);
            m_rewardState = new RewardState(this, context);
            m_gameOverState = new GameOverState();

            m_stateMachine = new StateMachine<StateMachineTriggers>(m_startupState);
            m_stateMachine.Configure(m_startupState)
                .Permit(StateMachineTriggers.StartupComplete, m_prepareRoomState);

            m_stateMachine.Configure(m_prepareRoomState)
                .Permit(StateMachineTriggers.RoomPrepared, m_travelState);

            m_stateMachine.Configure(m_travelState)
                .Permit(StateMachineTriggers.TravelCompleted_Enter, m_battleState)
                .Permit(StateMachineTriggers.TravelCompleted_Exit, m_prepareRoomState);

            m_stateMachine.Configure(m_battleState)
                .Permit(StateMachineTriggers.BattleWon, m_rewardState)
                .Permit(StateMachineTriggers.BattleLost, m_gameOverState);

            m_stateMachine.Configure(m_rewardState)
                .Permit(StateMachineTriggers.RewardPicked, m_travelState);

            m_stateMachine.Configure(m_gameOverState);
        }

        public async Task Start() => await m_startupState.EnterAsync();

        public async Task FireTrigger(StateMachineTriggers trigger, StatePayload payload = null) => await m_stateMachine.FireAsync(trigger, payload);
    }
}