using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachines.States;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachines
{
    public class RewardState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;

        public RewardState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;
        }

        public Task EnterAsync(StatePayload payload)
        {
            m_context.BattleReward.Calculate();
            m_context.Player.Animation.PlayIdleAnimation();

            m_context.UI.RewardUI.OnRewardPicked += HandleOnRewardPicked;

            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            m_context.UI.RewardUI.OnRewardPicked -= HandleOnRewardPicked;
            return Task.CompletedTask; 
        }

        private async void HandleOnRewardPicked(object sender, OnRewardPickedArgs e)
        {
            TravelStatePayload payload = new TravelStatePayload(TravelReason.ExitRoom);
            await m_stateMachine.FireTrigger(StateMachineTriggers.RewardPicked, payload);
        }
    }
}