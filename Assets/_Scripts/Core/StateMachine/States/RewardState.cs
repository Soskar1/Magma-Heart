using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.SceneLoading;
using MagmaHeart.Core.StateMachine.States;
using MagmaHeart.Core.TutorialSystem;
using MagmaHeart.StateMachine;
using System.Collections.Generic;
using System.Linq;
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
            m_context.Tutorial.Model.TrySetFlag(TutorialFlags.ArtifactsExplained);
            m_context.Player.Animation.PlayIdleAnimation();
            
            List<ArtifactData> rewards = m_context.RewardService.GenerateRewards();

            if (rewards.Count > 0)
            {
                // TODO: after implementing more artifacts, we need to randomize rewards
                m_rewardUI.DisplayRewards(rewards.Take(3));
                m_rewardUI.OnRewardPicked += HandleOnRewardPicked;

                return Task.CompletedTask;
            }

            return LeaveRoom();
        }

        public Task ExitAsync()
        {
            m_rewardUI.OnRewardPicked -= HandleOnRewardPicked;
            return Task.CompletedTask; 
        }

        private async void HandleOnRewardPicked(object sender, OnRewardPickedArgs e) => await LeaveRoom();

        private async Task LeaveRoom()
        {
            TravelStatePayload payload = new TravelStatePayload(TravelReason.ExitRoom);
            await m_stateMachine.FireTrigger(StateMachineTriggers.RewardPicked, payload);
        }
    }
}