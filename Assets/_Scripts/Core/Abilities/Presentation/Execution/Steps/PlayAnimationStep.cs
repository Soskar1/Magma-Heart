using MagmaHeart.Core.Entities;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    public class PlayAnimationStep : IAbilityExecutionStep
    {
        private readonly AnimationType m_animation;

        public PlayAnimationStep(AnimationType animationType)
        {
            m_animation = animationType;
        }

        public Task Run(AbilityExecutionContext context)
        {
            context.World.TryGetEntity(context.ExecutorId, out Entity executor);
            executor.Animation.PlayAnimation(m_animation);

            return Task.CompletedTask;
        }
    }
}
