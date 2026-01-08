using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachine.States;
using MagmaHeart.StateMachine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.Core.StateMachine
{
    public class RewardState : IState
    {
        private readonly MagmaHeartStateMachine m_stateMachine;
        private readonly MagmaHeartContext m_context;
        private readonly RewardUI m_rewardUI;

        public RewardState(MagmaHeartStateMachine stateMachine, MagmaHeartContext context)
        {
            m_stateMachine = stateMachine;
            m_context = context;
            m_rewardUI = m_context.UI.RewardUI;
        }

        public Task EnterAsync(StatePayload payload)
        {
            IEnumerable<ArtifactData> rewards = m_context.RewardService.GenerateRewards();
            m_rewardUI.DisplayRewards(rewards);
            m_rewardUI.OnRewardPicked += HandleOnRewardPicked;

            m_context.Player.Animation.PlayIdleAnimation();


            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            m_rewardUI.OnRewardPicked -= HandleOnRewardPicked;
            return Task.CompletedTask; 
        }

        private async void HandleOnRewardPicked(object sender, OnRewardPickedArgs e)
        {
            TravelStatePayload payload = new TravelStatePayload(TravelReason.ExitRoom);
            await m_stateMachine.FireTrigger(StateMachineTriggers.RewardPicked, payload);
        }
    }
}