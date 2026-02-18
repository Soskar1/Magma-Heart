using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem.Actions.Commands;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Presenters
{
    public class ApplyDamageCommandPresenter : IBoardCommandPresenter<ApplyDamageCommand>
    {
        private readonly CommandRunner m_commandRunner;

        public ApplyDamageCommandPresenter(CommandRunner commandRunner)
        {
            m_commandRunner = commandRunner;
        }

        public async Task Present(Room room, ApplyDamageCommand command, CancellationToken token)
        {
            // Attack animation
            room.TryGetEntity(command.ExecutorId, out Entity executor);

            int animationHash = executor.Animation.PlayAttackAnimation();
            Task animationEndTask = executor.Animation.WaitForStateToFinish(animationHash);

            // Wait for event
            await executor.Animation.GetAnimationTriggerTask();

            // Apply damage
            m_commandRunner.Apply(room, command);

            // Wait for animation end
            if (!animationEndTask.IsCompleted)
                await animationEndTask;

            executor.Animation.PlayIdleAnimation();
        }
    }
}
