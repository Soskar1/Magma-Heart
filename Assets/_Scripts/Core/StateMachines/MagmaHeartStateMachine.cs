using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachines.States;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public enum StateMachineStates
    {
        Startup,
        Travel,
        Battle,
        Reward,
        GameOver
    }

    public enum StateMachineTriggers
    {
        StartupComplete,
        BattleStarted,
        BattleLost,
        BattleWon,
        RewardPicked
    }

    public class MagmaHeartStateMachine
    {
        private readonly StateMachine<StateMachineStates, StateMachineTriggers> m_stateMachine;

        private readonly StartupState m_startupState;
        private readonly TravelState m_travelState;
        private readonly BattleState m_battleState;
        private readonly RewardState m_rewardState;
        private readonly GameOverState m_gameOverState;
        
        public MagmaHeartStateMachine(MagmaHeartContext context, int travelSpeed)
        {
            m_startupState = new StartupState(this, context);
            m_travelState = new TravelState(this, context, travelSpeed);
            m_battleState = new BattleState(this, context);
            m_rewardState = new RewardState(this, context);
            m_gameOverState = new GameOverState();

            m_stateMachine = new StateMachine<StateMachineStates, StateMachineTriggers>(StateMachineStates.Startup);
            m_stateMachine.Configure(StateMachineStates.Startup)
                .Permit(StateMachineTriggers.StartupComplete, StateMachineStates.Travel)
                .OnEntryAsync(m_startupState.EnterAsync)
                .OnExitAsync(m_startupState.ExitAsync);

            m_stateMachine.Configure(StateMachineStates.Travel)
                .Permit(StateMachineTriggers.BattleStarted, StateMachineStates.Battle)
                .OnEntryAsync(m_travelState.EnterAsync)
                .OnExitAsync(m_travelState.ExitAsync);

            m_stateMachine.Configure(StateMachineStates.Battle)
                .Permit(StateMachineTriggers.BattleWon, StateMachineStates.Reward)
                .Permit(StateMachineTriggers.BattleLost, StateMachineStates.GameOver)
                .OnEntryAsync(m_battleState.EnterAsync)
                .OnExitAsync(m_battleState.ExitAsync);

            m_stateMachine.Configure(StateMachineStates.Reward)
                .Permit(StateMachineTriggers.RewardPicked, StateMachineStates.Travel)
                .OnEntryAsync(m_rewardState.EnterAsync)
                .OnExitAsync(m_rewardState.ExitAsync);

            m_stateMachine.Configure(StateMachineStates.GameOver)
                .OnEntryAsync(m_gameOverState.EnterAsync)
                .OnExitAsync(m_gameOverState.ExitAsync);
        }

        public async Task Start() => await m_startupState.EnterAsync();

        public async Task FireTrigger(StateMachineTriggers trigger) => await m_stateMachine.FireAsync(trigger);
    }
}